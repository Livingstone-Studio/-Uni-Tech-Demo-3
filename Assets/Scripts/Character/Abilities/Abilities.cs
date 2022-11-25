using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class Abilities : MonoBehaviour
{
    [SerializeField] protected List<AbilitySO> abilitySetOne = new List<AbilitySO>();

    [SerializeField] protected List<AbilitySO> abilitySetTwo = new List<AbilitySO>();

    protected List<List<AbilitySO>> abilitySets = new List<List<AbilitySO>>();

    protected CharacterStats stats;

    public int currentSetIndex = 0;

    protected virtual void Start()
    {
        stats = GetComponent<CharacterStats>();

        abilitySets.Add(abilitySetOne);
        abilitySets.Add(abilitySetTwo);
    }

    protected virtual void Update()
    {

    }

    public virtual void AbilityOne()
    {
        if (stats.GetResource() < abilitySets[currentSetIndex][0].cost) return;
    }

    public virtual void AbilityTwo()
    {
        if (stats.GetResource() < abilitySets[currentSetIndex][1].cost) return;
    }

    public virtual void AbilityThree()
    {
        if (stats.GetResource() < abilitySets[currentSetIndex][2].cost) return;
    }

    public virtual void AbilityFour()
    {
        if (stats.GetResource() < abilitySets[currentSetIndex][3].cost) return;
    }


    public virtual void SelectAbilitySet(int index)
    {
        if (index >= 0 && index < abilitySets.Count && abilitySets.Count > 0)
        {
            currentSetIndex = index;
        }
    }
}
