using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum ResourceType { MANA, RAGE, ENERGY, NONE }
public class CharacterStats : MonoBehaviour
{
    public static CharacterStats Player { set; get; }

    [SerializeField] private UnityEvent onTargetDie;

    [SerializeField] private bool isPlayer = false;

    [SerializeField] private GameObject attackPanel;

    internal bool targettedByPlayer = false;

    [SerializeField] private SpriteRenderer sR;

    private CharacterStats target;

    [SerializeField] private Sprite portrait;

    [SerializeField] private Animator animator;

    [SerializeField] private GameObject dmgTextPrefab;
    [SerializeField] private Transform dmgTextSpawnPos;

    [SerializeField] private CharacterAttacking characterAttacking;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private PlayerController playerController;

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    private float health;

    [Header("Resource Settings")]
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private float maxResource = 100f;
    private float defaultMaxResource;
    private float resource;

    [Header("Energy Settings")]
    [SerializeField] private float energyRegenTime = 0f;
    private float energyTimer = 0f;

    private float energyRegenRate = 1f;

    [Header("Buff Settings")]

    [SerializeField] private List<BuffSO> activeBuffsDebuffs = new List<BuffSO>();

    [Header("Combo Points")]

    [SerializeField] private int maxCP = 5;
    private int currentCP = 0;

    [Header("Damage Reduction")]

    [SerializeField][Range(0, 100)] private int phyDmgReduction = 25;
    [SerializeField] [Range(0, 100)] private int magDmgReduction = 25;

    private bool scared = false;
    private float scaredTime = 0f;
    private Vector2 scaredDir = Vector2.zero;

    private Vector2 spawnPos;

    private void Awake()
    {
        if (isPlayer)
        {
            Player = this;
        }
    }

    private void Start()
    {
        health = maxHealth;
        resource = maxResource;
        defaultMaxResource = maxResource;

        if (isPlayer) CharacterPanel.Player.Setup(this);

        spawnPos = transform.position;
    }

    private void Update()
    {
        if (characterAttacking) characterAttacking.attackRate = 1;
        if (characterMovement) characterMovement.moveRate = 1;
        energyRegenRate = 1;
        maxResource = defaultMaxResource;
        if (sR) sR.color = new Color(1, 1, 1, 1);
        scared = false;

        if (target == CharacterStats.Player && !isPlayer && CharacterStats.Player.GetHealth() <= 0)
        {
            RemoveTarget();
        }

        foreach (BuffSO buff in activeBuffsDebuffs)
        {
            if (buff == null) return;

            // RUN BUFF ON TICK EVENTS

            BuffOnTickEvent(buff);
        }

        if (scared)
        {
            if (playerController) playerController.enabled = false;

            if (characterMovement)
            {
                scaredTime += Time.deltaTime;

                if (scaredTime >= 1f)
                {
                    scaredDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    scaredTime = 0;
                }

                characterMovement.SetMoveDir(scaredDir.normalized);
            }
        }
        else
        {
            if (playerController) playerController.enabled = true;

            scaredDir = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            scaredTime = 0;
        }

        ManageResources();

        if (isPlayer && attackPanel)
        {
            if (target != null && target != this && !attackPanel.activeInHierarchy)
            {
                attackPanel.SetActive(true);
            }
            else if ((!target || target == this) && attackPanel.activeInHierarchy)
            {
                attackPanel.SetActive(false);
            }
        }

        if (!target) return;

        if (!target.isPlayer && !CharacterPanel.TargetTarget.gameObject.activeInHierarchy) CharacterPanel.TargetTarget.Setup(target.GetTarget());


    }

    #region Health

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    public void TakeDamage(float amount, bool poison = false)
    {
        if (poison && CheckForToxic())
        {
            amount *= 2;
        }
        else
        {
            amount *= ((float)(100 - phyDmgReduction)) / 100f;
        }

        health -= amount;

        if (dmgTextPrefab && dmgTextSpawnPos)
        {
            GameObject go = Instantiate(dmgTextPrefab, dmgTextSpawnPos);

            DmgText dmgt = go.GetComponent<DmgText>();

            if (dmgt) dmgt.SetText((int)amount, Color.white);

        }

        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health += amount;

        if (dmgTextPrefab && dmgTextSpawnPos)
        {
            GameObject go = Instantiate(dmgTextPrefab, dmgTextSpawnPos);

            DmgText dmgt = go.GetComponent<DmgText>();

            if (dmgt) dmgt.SetText((int)amount, Color.green);

        }

        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Die()
    {
        if (animator) animator.SetBool("dead", true);

        if (TryGetComponent<EnemyController>(out EnemyController eC)) eC.enabled = false;

        RemoveTarget();

        if (targettedByPlayer && !isPlayer)
        {
            CharacterStats.Player.RemoveTarget();

            Castbar.Instance.value = 0;
            Castbar.Instance.gameObject.SetActive(false);

            onTargetDie.Invoke();

            this.enabled = false;
        }

        if (isPlayer) StartCoroutine(Respawn());
    }

    #endregion

    #region Resources

    private void ManageResources()
    {
        switch (resourceType)
        {
            case ResourceType.MANA:
                break;
            case ResourceType.RAGE:
                break;
            case ResourceType.ENERGY:
                RegenerateEnergy();
                break;
        }
    }

    private void RegenerateEnergy()
    {
        energyTimer += Time.deltaTime * energyRegenRate;

        if (energyTimer >= energyRegenTime)
        {
            if (resource < maxResource)
            {
                UpdateResource(1);
            }
            energyTimer = 0f;
        }
    }

    public void UpdateResource(float amount)
    {
        resource += amount;

        if (resource >= maxResource) resource = maxResource;
        else if (resource <= 0) resource = 0;

        if (isPlayer) CharacterPanel.Player.SetResource();
    }

    public float GetMaxResource()
    {
        return maxResource;
    }

    public float GetResource()
    {
        return resource;
    }
    
    public ResourceType GetResourceType()
    {
        return resourceType;
    }

    public Sprite GetPortrait()
    {
        return portrait;
    }

    #endregion

    #region Buff System

    /// <summary>
    /// ONLY USE ON BUFFS ON THE CS OBJECT
    /// </summary>
    /// <param name="buff"></param>
    public void AddBuff(BuffSO buff)
    {
        if (!activeBuffsDebuffs.Contains(buff))
        {
            gameObject.AddComponent<Buff>();

            Buff[] bu = GetComponents<Buff>();

            Buff b = null;

            foreach (Buff bi in bu)
            {
                if (bi.GetBuff() == null)
                {
                    b = bi;
                    break;
                }
            }

            if (b == null) return;

            b.SetBuff(buff);

            activeBuffsDebuffs.Add(b.GetBuff());

            if (isPlayer) BuffPanel.Instance.AddBuff(buff);
            if (targettedByPlayer && BuffPanel.TargetPanel) BuffPanel.TargetPanel.AddBuff(buff);
        }
    }

    public void AddBuff(BuffSO buff, float time)
    {
        if (!activeBuffsDebuffs.Contains(buff))
        {
            gameObject.AddComponent<Buff>();

            Buff[] bu = GetComponents<Buff>();

            Buff b = null;

            foreach (Buff bi in bu)
            {
                if (bi.GetBuff() == null)
                {
                    b = bi;
                    break;
                }
            }

            if (b == null) return;

            b.SetBuff(buff, time);

            activeBuffsDebuffs.Add(b.GetBuff());

            if (isPlayer) BuffPanel.Instance.AddBuff(buff);
            if (targettedByPlayer && BuffPanel.TargetPanel) BuffPanel.TargetPanel.AddBuff(buff);
        }
    }

    /// <summary>
    /// ONLY USE ON BUFFS ON THE CS OBJECT
    /// </summary>
    /// <param name="buff"></param>
    public void RemoveBuff(BuffSO buff)
    {
        if (activeBuffsDebuffs.Contains(buff)) activeBuffsDebuffs.Remove(buff);

        if (isPlayer) BuffPanel.Instance.RemoveBuff(buff);

        Buff b = GetIndexOfBuff(buff);

        if (b)
        {
            b.Remove();
        }
    }

    private void BuffOnTickEvent(BuffSO buff)
    {
        if (buff.affectAttackRate)
        {
            if (characterAttacking) characterAttacking.attackRate = buff.newRate;
        }

        if (buff.affectMovementRate)
        {
            if (characterMovement) characterMovement.moveRate = buff.newRate;
        }

        if (buff.affectResourceRate)
        {
            energyRegenRate = buff.newRate2;
        }

        if (buff.affectResourceAmount)
        {
            maxResource = defaultMaxResource + buff.newResourceMax;
        }

        if (buff.affectSpriteTranspancey)
        {
            if (sR) sR.color = new Color(1, 1, 1, .5f);
        }

        if (buff.characterPanic) scared = true;
    }

    private Buff GetIndexOfBuff(BuffSO buff)
    {
        Buff[] b = GetComponents<Buff>();

        for (int i = 0; i < b.Length; i++)
        {
            if (b[i].GetBuff() == buff)
            {
                return b[i];
            }
        }

        return null;
    }

    public Buff[] GetAllBuffs()
    {
        return GetComponents<Buff>();
    }

    public Buff GetCharacterVersion(Buff buff)
    {
        Buff[] buffs = GetAllBuffs();

        for (int i = 0; i < buffs.Length; i++)
        {
            if (buffs[i].GetBuff() == buff.GetBuff())
            {
                return buffs[i];
            }
        }

        if (targettedByPlayer) BuffPanel.TargetPanel.RemoveBuff(buff);

        return null;
    }
    #endregion

    #region Targetting

    public void SetTarget(CharacterStats cs)
    {
        if (target != null) RemoveTarget();

        target = cs;

        if (isPlayer)
        {
            CharacterPanel.Target.Setup(target);
            target.targettedByPlayer = true;
        }

        if (!target.isPlayer && target) CharacterPanel.TargetTarget.Setup(target.GetTarget());
    }

    public void RemoveTarget()
    {
        if (target != null) target.targettedByPlayer = false;
        target = null;
        if (isPlayer) CharacterPanel.Target.gameObject.SetActive(false);
        if (isPlayer) CharacterPanel.TargetTarget.gameObject.SetActive(false);
    }

    public CharacterStats GetTarget()
    {
        return target;
    }


    public void SetSelfTarget()
    {
        SetTarget(this);
    }

    public void SetTargetTarget()
    {
        if (!target) return;
        if (!target.GetTarget()) return;

        SetTarget(target.GetTarget());
    }
    
    #endregion

    public void AddComboPoint(int amount)
    {
        currentCP += amount;

        if (currentCP >= maxCP)
        {
            currentCP = maxCP;
        }

        if (targettedByPlayer) CharacterPanel.Target.EditCPText(currentCP);
    }

    public int GetCurrentCPAmount()
    {
        return currentCP;
    }

    public void ResetCP()
    {
        currentCP = 0;
    }

    public bool CheckForToxic()
    {
        foreach (BuffSO buff in activeBuffsDebuffs)
        {
            if (buff.toxic)
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerator Respawn()
    {
        //playerController.enabled = false;

        yield return new WaitForSeconds(3);

        //playerController.enabled = true;

        Heal(maxHealth);

        transform.position = spawnPos;

        if (animator)
        {
            animator.SetBool("dead", false);
            animator.SetTrigger("Respawn");
        }
    }
}
