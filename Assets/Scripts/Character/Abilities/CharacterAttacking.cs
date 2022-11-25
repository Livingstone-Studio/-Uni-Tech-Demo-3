using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAttacking : MonoBehaviour
{

    public bool canAttack = false;

    [SerializeField] private Transform shootpoint;
    [SerializeField] private GameObject spitGO;
    [SerializeField] private GameObject toxicSplitGO;

    [SerializeField] private float minDmg = 11;
    [SerializeField] private float maxDmg = 19;

    [SerializeField] private float attackSpeed = 0.6f;
    private float attackTimer = 0f;

    [SerializeField] private float minDmg2 = 6;
    [SerializeField] private float maxDmg2 = 13;

    [SerializeField] private float attackSpeed2 = -1f;
    private float attackTimer2 = 0f;

    [SerializeField] private Animator animator;

    public Triggered triggered;

    [SerializeField] private LayerMask characterLayer;

    public CharacterStats characterStats;

    [SerializeField] [Range(0, 100)] private int chanceToHit = 100;
    [SerializeField] [Range(0, 100)] private int chanceToCrit = 0;
    [SerializeField] private float critMultiplier = 2f;

    [SerializeField] private BuffSO set1AttackSpeedBuff;

    [SerializeField] private Dagger jess;
    [SerializeField] private Dagger sarah;

    internal float attackRate = 1f;

    public bool abilityActivated = false;

    private void Update()
    {
        if (!triggered) return;

        if (animator.GetBool("CombatReady") != canAttack) animator.SetBool("CombatReady", canAttack);

        if (triggered.triggered && canAttack && !abilityActivated)
        {
            attackTimer += (Time.deltaTime * attackRate);

            if (attackTimer > attackSpeed && animator)
            {
                attackTimer = 0;
                animator.SetTrigger("Attacking");
            }

            if (characterStats.GetHealth() <= 0) abilityActivated = false;

            if (attackSpeed2 > 0)
            {

                attackTimer2 += Time.deltaTime;

                if (attackTimer2 > attackSpeed2 && animator)
                {
                    attackTimer2 = 0;
                    animator.SetTrigger("Attacking2");
                }
            }
        }
        else
        {
            attackTimer = 0;
        }
    }

    public void ToggleAttackMode(Image image)
    {
        canAttack = !canAttack;

        if (canAttack)
        {
            image.color = Color.red;
        }
        else
        {
            image.color = Color.white;
        }
    }

    public void DisableAttackMode(Image image)
    {
        canAttack = false;

        if (canAttack)
        {
            image.color = Color.red;
        }
        else
        {
            image.color = Color.white;
        }
    }

    public void Attack()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 3f, Vector2.up, 0f, characterLayer);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject != transform.parent.gameObject)
                {
                    if (hit.transform.TryGetComponent<CharacterStats>(out CharacterStats stats))
                    {
                        if (characterStats)
                        {
                            if (characterStats.GetTarget() == stats && stats.GetHealth() > 0)
                            {
                                int hitchance = Random.Range(0, 101);

                                if (hitchance <= chanceToHit)
                                {
                                    int critChance = Random.Range(0, 101);

                                    if (critChance <= chanceToCrit)
                                    {
                                        stats.TakeDamage((Random.Range(minDmg, maxDmg)) * critMultiplier);
                                    }
                                    else
                                    {
                                        stats.TakeDamage(Random.Range(minDmg, maxDmg));
                                    }
                                }
                                else
                                {
                                    stats.TakeDamage(0);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void Attack2()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 3f, Vector2.up, 0f, characterLayer);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject != transform.parent.gameObject)
                {
                    if (hit.transform.TryGetComponent<CharacterStats>(out CharacterStats stats))
                    {
                        if (characterStats)
                        {
                            if (characterStats.GetTarget() == stats && stats.GetHealth() > 0)
                            {
                                int hitchance = Random.Range(0, 101);

                                if (hitchance <= chanceToHit)
                                {
                                    int critChance = Random.Range(0, 101);

                                    if (critChance <= chanceToCrit)
                                    {
                                        stats.TakeDamage((Random.Range(minDmg2, maxDmg2)) * critMultiplier);
                                    }
                                    else
                                    {
                                        if (sarah)
                                        {
                                            switch (sarah.posionActive)
                                            {
                                                case PosionActive.INSTANT:
                                                    stats.TakeDamage(Random.Range(38, 52));
                                                    break;
                                                case PosionActive.DEADLY:
                                                    break;
                                                case PosionActive.CRIPPLING:
                                                    break;
                                                case PosionActive.MINDNUMBING:
                                                    break;
                                            }
                                        }


                                        stats.TakeDamage(Random.Range(minDmg2, maxDmg2));
                                    }
                                }
                                else
                                {
                                    stats.TakeDamage(0);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void SinisterStrike()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 3f, Vector2.up, 0f, characterLayer);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject != transform.parent.gameObject)
                {
                    if (hit.transform.TryGetComponent<CharacterStats>(out CharacterStats stats))
                    {
                        if (characterStats)
                        {
                            if (characterStats.GetTarget() == stats && stats.GetHealth() > 0)
                            {
                                float dmgAmount = Random.Range(minDmg, maxDmg);

                                dmgAmount += Random.Range(20, 35);


                                stats.TakeDamage(dmgAmount);
                                stats.AddComboPoint(1);
                            }
                        }
                    }
                }
            }
        }

        abilityActivated = false;
    }

    public void Eviserate()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 3f, Vector2.up, 0f, characterLayer);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject != transform.parent.gameObject)
                {
                    if (hit.transform.TryGetComponent<CharacterStats>(out CharacterStats stats))
                    {
                        if (characterStats)
                        {
                            if (characterStats.GetTarget() == stats && stats.GetHealth() > 0)
                            {
                                float dmgAmount = 0;

                                switch (stats.GetCurrentCPAmount())
                                {
                                    case 1:
                                        dmgAmount = Random.Range(40, 75);
                                        break;
                                    case 2:
                                        dmgAmount = Random.Range(89, 112);
                                        break;
                                    case 3:
                                        dmgAmount = Random.Range(135, 167);
                                        break;
                                    case 4:
                                        dmgAmount = Random.Range(184, 209);
                                        break;
                                    case 5:
                                        dmgAmount = Random.Range(224, 257);
                                        break;
                                }

                                stats.TakeDamage(dmgAmount);

                                stats.ResetCP();
                            }
                        }
                    }
                }
            }
        }

        abilityActivated = false;
    }

    public void Mutilate()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 3f, Vector2.up, 0f, characterLayer);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject != transform.parent.gameObject)
                {
                    if (hit.transform.TryGetComponent<CharacterStats>(out CharacterStats stats))
                    {
                        if (characterStats)
                        {
                            if (characterStats.GetTarget() == stats && stats.GetHealth() > 0)
                            {
                                float dmgAmount = Random.Range(minDmg, maxDmg);

                                int timeDur = 6 + (3 * stats.GetCurrentCPAmount());

                                if (set1AttackSpeedBuff) characterStats.AddBuff(set1AttackSpeedBuff, timeDur);

                                stats.TakeDamage(dmgAmount);

                                stats.ResetCP();
                            }
                        }
                    }
                }
            }
        }

        abilityActivated = false;
    }

    public void Slice()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 3f, Vector2.up, 0f, characterLayer);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject != transform.parent.gameObject)
                {
                    if (hit.transform.TryGetComponent<CharacterStats>(out CharacterStats stats))
                    {
                        if (characterStats)
                        {
                            if (characterStats.GetTarget() == stats && stats.GetHealth() > 0)
                            {
                                stats.TakeDamage(Random.Range(minDmg, maxDmg));
                                stats.AddComboPoint(1);
                            }
                        }
                    }
                }
            }
        }
    }

    public void Dice()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 3f, Vector2.up, 0f, characterLayer);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject != transform.parent.gameObject)
                {
                    if (hit.transform.TryGetComponent<CharacterStats>(out CharacterStats stats))
                    {
                        if (characterStats)
                        {
                            if (characterStats.GetTarget() == stats && stats.GetHealth() > 0)
                            {
                                stats.TakeDamage(Random.Range(minDmg2, maxDmg2));
                                stats.AddComboPoint(1);
                            }
                        }
                    }
                }
            }
        }

        abilityActivated = false;
    }

    public void Kick()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 3f, Vector2.up, 0f, characterLayer);

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject != transform.parent.gameObject)
                {
                    if (hit.transform.TryGetComponent<CharacterStats>(out CharacterStats stats))
                    {
                        if (characterStats)
                        {
                            if (characterStats.GetTarget() == stats && stats.GetHealth() > 0)
                            {
                                stats.TakeDamage(14);
                            }
                        }
                    }
                }
            }
        }

        abilityActivated = false;
    }

    public void Spit()
    {
        if (!spitGO || !shootpoint) return;

        Instantiate(spitGO, shootpoint.position, Quaternion.identity);
    }

    public void ToxicSplit()
    {
        if (!toxicSplitGO || !shootpoint) return;

        Instantiate(toxicSplitGO, shootpoint.position, Quaternion.identity);
    }
}