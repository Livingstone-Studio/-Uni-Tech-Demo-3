using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{

    CharacterStats characterStats;
    
    [SerializeField] protected float aggroRadius;

    [SerializeField] protected LayerMask characterLayer;

    [SerializeField] protected float attackTime = 1.5f;

    [SerializeField] protected float maxWanderTime = 1.5f;
    protected float wanderTimer = 0f;
    Vector2 wanderDir;

    [SerializeField] private Animator animator;

    [SerializeField] private BuffSO enraged;

    private float attackTimer = 0f;

    private int attackCount = 0;

    private bool buffed = false;

    private float splitMaxTime = 2.5f;
    private float splitTimer = 0f;
    [SerializeField] private CharacterMovement characterMovement;

    [SerializeField] private BuffSO fearSO;
    int num;

    protected virtual void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        Castbar.Instance.transform.gameObject.SetActive(false);
        wanderDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    protected virtual void Update()
    {
        if (characterStats.GetTarget() == null)
        {
            CheckAggro();

            wanderTimer += Time.deltaTime;

            if (wanderTimer >= maxWanderTime)
            {
                wanderDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                wanderTimer = 0f;
            }

            characterMovement.SetMoveDir(wanderDir.normalized);
        }
        else
        {
            AggroBehaviour();
            characterMovement.SetMoveDir(Vector2.zero);
        }
    }

    protected virtual void CheckAggro()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, aggroRadius, Vector2.up, 0f, characterLayer);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.CompareTag("Player") && hit.transform.TryGetComponent<CharacterStats>(out CharacterStats cs))
                {
                    characterStats.SetTarget(cs);
                }
            }
        }
    }

    protected virtual void AggroBehaviour()
    {
        if (attackCount < 3)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer > attackTime)
            {
                animator.SetTrigger("RangedAttack");

                attackTimer = 0;
                attackCount++;

                if (attackCount >= 3)
                {
                    num = Random.Range(0, 101);
                }
            }
        }
        else
        {

            if (num < 40 && (characterStats.GetHealth()/characterStats.GetMaxHealth()) < .6f)
            {
                if (fearSO)
                {
                    Castbar.Instance.transform.gameObject.SetActive(false);
                    CharacterStats.Player.AddBuff(fearSO);
                    attackCount = 0;
                }
            }
            else
            {
                Toxic();
            }
        }

        if ((characterStats.GetHealth() / characterStats.GetMaxHealth()) < .25f && !buffed && enraged)
        {
            characterStats.AddBuff(enraged);
            buffed = true;
        }
    }

    private void Toxic()
    {
        Castbar.Instance.transform.gameObject.SetActive(true);

        splitTimer += Time.deltaTime;

        Castbar.Instance.value = splitTimer / splitMaxTime;

        if (splitTimer >= splitMaxTime)
        {
            animator.SetTrigger("Toxic");
            splitTimer = 0;
            attackCount = 0;
            Castbar.Instance.transform.gameObject.SetActive(false);
        }
    }
}
