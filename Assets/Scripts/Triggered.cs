using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggered : MonoBehaviour
{
    public bool triggered = false;

    public CharacterStats characterStats;

    public CharacterStats cs;

    public Triggered t;

    private void Update()
    {
        if (characterStats)
        {
            if ((characterStats.GetHealth() <= 0))
            {
                triggered = false;

            }

            if (t)
            {
                if (t.characterStats != characterStats.GetTarget() || (t.characterStats.GetHealth() <= 0))
                {
                    triggered = false;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent<Triggered>(out Triggered cS))
            {
                if (characterStats.GetTarget() == cS.characterStats && cS.characterStats.GetHealth() > 0)
                {
                    triggered = true;

                    t = cS;
                    cs = t.characterStats;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent<Triggered>(out Triggered cS))
            {
                if (characterStats.GetTarget() == cS.characterStats)
                {
                    triggered = false;
                    t = null;
                    cs = null;
                }
            }
        }
    }
}
