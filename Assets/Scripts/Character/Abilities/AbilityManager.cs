using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance { set; get; }

    [SerializeField] private Abilities abilities;

    private void Awake()
    {
        Instance = this;
    }

    public void SetAbilities(Abilities _abilities)
    {
        abilities = _abilities;
    }

    public void AbilityOne()
    {
        abilities.AbilityOne();
    }

    public void AbilityTwo()
    {
        abilities.AbilityTwo();
    }

    public void AbilityThree()
    {
        abilities.AbilityThree();
    }

    public void AbilityFour()
    {
        abilities.AbilityFour();
    }
}
