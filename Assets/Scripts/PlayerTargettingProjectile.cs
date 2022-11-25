using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargettingProjectile : MonoBehaviour
{

    [SerializeField] [Range(0, 100)] private int chanceToHit = 20;
    [SerializeField][Range(0, 100)] private int chanceToCrit = 20;

    [SerializeField] private float critMultiplier = 2.5f;

    [SerializeField] private float minDmg = 18;
    [SerializeField] private float maxDmg = 35;

    [SerializeField] private BuffSO toxicSO;

    // Update is called once per frame
    void Update()
    {
        if (CharacterStats.Player) transform.position = Vector2.MoveTowards(transform.position, CharacterStats.Player.transform.position, Time.deltaTime * 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<CharacterStats>(out CharacterStats stats))
            {
                if (toxicSO)
                {
                    ToxicAttack(stats);
                    Destroy(gameObject);
                }
                else
                {
                    RegularAttack(stats);
                    PosionDamage(stats);
                    Destroy(gameObject);
                }
            }

        }
    }

    private static void PosionDamage(CharacterStats stats)
    {
        int posionchance = Random.Range(0, 101);

        if (posionchance <= 35)
        {
            stats.TakeDamage(Random.Range(10, 30), true);
        }
    }

    private void RegularAttack(CharacterStats stats)
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

    private void ToxicAttack(CharacterStats stats)
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

        stats.AddBuff(toxicSO);
    }
}
