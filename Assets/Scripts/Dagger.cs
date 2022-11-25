using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PosionActive { NONE, INSTANT, DEADLY, CRIPPLING, MINDNUMBING }
public class Dagger : MonoBehaviour
{

    public PosionActive posionActive = PosionActive.NONE;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (animator)
        {
            if (!animator.GetBool("Posioned") && posionActive != PosionActive.NONE)
            {
                animator.SetBool("Posioned", true);
            }
            else if (animator.GetBool("Posioned") && posionActive == PosionActive.NONE)
            {
                animator.SetBool("Posioned", false);
            }
        }
    }

}
