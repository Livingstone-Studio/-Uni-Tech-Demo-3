using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RogueAbilities : Abilities
{

    public static RogueAbilities Instance { set; get; }

    [SerializeField] private List<AbilityButton> abilityButtons = new List<AbilityButton>();

    [SerializeField] private Animator animator;
    [SerializeField] private CharacterAttacking characterAttacking;

    [SerializeField] private BuffSO sprintBuff;
    [SerializeField] private BuffSO evasionBuff;
    [SerializeField] private BuffSO adrenalineBuff;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    protected override void Start()
    {
        base.Start();
        SetupAbilityButtons();
    }

    private void SetupAbilityButtons()
    {
        for (int i = 0; i < abilityButtons.Count; i++)
        {
            abilityButtons[i].Setup(abilitySets[currentSetIndex][i]);
        }
    }

    protected override void Update()
    {
        for (int i = 0; i < abilitySetOne.Count; i++)
        {
            if (abilitySetOne[i].cost > stats.GetResource())
            {
                abilityButtons[i].SetButton(false);
                abilityButtons[i].icon.color = Color.red;
            }
            else
            {
                abilityButtons[i].SetButton(true);
            }
        }

        base.Update();
    }

    public override void AbilityOne()
    {
        if (stats.GetResource() < abilitySets[currentSetIndex][0].cost) return;

        base.AbilityOne();


        switch (currentSetIndex)
        {
            case 0:
                SinisterStrike();
                break;
            case 1:
                Sprint();
                break;
        }
    }

    public override void AbilityTwo()
    {
        if (stats.GetResource() < abilitySets[currentSetIndex][1].cost) return;

        base.AbilityTwo();


        switch (currentSetIndex)
        {
            case 0:
                Eviserate();
                break;
            case 1:
                Evasion();
                break;
        }
    }

    public override void AbilityThree()
    {
        if (stats.GetResource() < abilitySets[currentSetIndex][2].cost) return;

        base.AbilityThree();


        switch (currentSetIndex)
        {
            case 0:
                Mutilate();
                break;
            case 1:
                Kick();
                break;
        }
    }

    public override void AbilityFour()
    {
        if (stats.GetResource() < abilitySets[currentSetIndex][3].cost) return;

        base.AbilityFour();


        switch (currentSetIndex)
        {
            case 0:
                SlicenDice();
                break;
            case 1:
                AdrenalineRush();
                break;
        }
    }

    public override void SelectAbilitySet(int index)
    {
        base.SelectAbilitySet(index);

        if (index >= 0 && index < abilitySets.Count && abilitySets.Count > 0)
        {
            currentSetIndex = index;
            SetupAbilityButtons();
        }
    }

    #region Set One

    public void SinisterStrike()
    {
        if (characterAttacking)
        {
            if (characterAttacking.abilityActivated) return;
            if (characterAttacking.triggered.triggered)
            {
                if (animator) animator.SetTrigger("Sinister");
                characterAttacking.abilityActivated = true;
                stats.UpdateResource(-abilitySets[currentSetIndex][0].cost);
                abilityButtons[0].StartCooldownOne(abilitySets[currentSetIndex][0].cooldown);
            }
        }
    }

    public void Eviserate()
    {
        if (characterAttacking)
        {
            if (characterAttacking.abilityActivated) return;
            if (!characterAttacking.triggered) return;

            if (!characterAttacking.triggered.cs) return;


            if (characterAttacking.triggered.triggered && characterAttacking.triggered.cs.GetCurrentCPAmount() > 0)
            {
                if (animator) animator.SetTrigger("Eviserate");
                characterAttacking.abilityActivated = true;
                stats.UpdateResource(-abilitySets[currentSetIndex][1].cost);
                abilityButtons[1].StartCooldownOne(abilitySets[currentSetIndex][1].cooldown);
            }
        }
    }

    public void SlicenDice()
    {
        if (characterAttacking)
        {
            if (characterAttacking.abilityActivated) return;
            if (characterAttacking.triggered.triggered)
            {
                if (animator) animator.SetTrigger("SlicenDice");
                characterAttacking.abilityActivated = true;
                stats.UpdateResource(-abilitySets[currentSetIndex][3].cost);
                abilityButtons[2].StartCooldownOne(abilitySets[currentSetIndex][3].cooldown);
            }
        }
    }

    public void Mutilate()
    {
        if (characterAttacking)
        {
            if (characterAttacking.abilityActivated) return;
            if (!characterAttacking.triggered) return;
            if (!characterAttacking.triggered.cs) return;

            if (characterAttacking.triggered.triggered && characterAttacking.triggered.cs.GetCurrentCPAmount() > 0)
            {
                if (animator) animator.SetTrigger("Mutilate");
                characterAttacking.abilityActivated = true;
                stats.UpdateResource(-abilitySets[currentSetIndex][2].cost);
                abilityButtons[3].StartCooldownOne(abilitySets[currentSetIndex][2].cooldown);
            }
        }
    }

    #endregion

    #region Set Two

    public void Sprint()
    {
        if (characterAttacking)
        {
            if (characterAttacking.abilityActivated) return;

            stats.AddBuff(sprintBuff);

            stats.UpdateResource(-abilitySets[currentSetIndex][0].cost);
            abilityButtons[0].StartCooldownTwo(abilitySets[currentSetIndex][0].cooldown);
        }
    }

    public void Evasion()
    {
        if (characterAttacking)
        {
            if (characterAttacking.abilityActivated) return;

            stats.AddBuff(evasionBuff);

            stats.UpdateResource(-abilitySets[currentSetIndex][1].cost);
            abilityButtons[1].StartCooldownTwo(abilitySets[currentSetIndex][1].cooldown);
        }
    }
    public void Kick()
    {
        if (characterAttacking)
        {
            if (characterAttacking.abilityActivated) return;

            if (animator) animator.SetTrigger("Kick");


            characterAttacking.abilityActivated = true;
            stats.UpdateResource(-abilitySets[currentSetIndex][2].cost);
            abilityButtons[2].StartCooldownTwo(abilitySets[currentSetIndex][2].cooldown);
        }
    }
    public void AdrenalineRush()
    {
        if (characterAttacking)
        {
            if (characterAttacking.abilityActivated) return;

            stats.AddBuff(adrenalineBuff);

            stats.UpdateResource(-abilitySets[currentSetIndex][3].cost);
            abilityButtons[3].StartCooldownTwo(abilitySets[currentSetIndex][3].cooldown);
        }
    }
    #endregion

    public void GlobalCooldown(float cooldown)
    {
        if (cooldown <= 0)
        {
            foreach(AbilityButton button in abilityButtons)
            {
                button.StartCooldownOne(1.5f);
                button.StartCooldownTwo(1.5f);
            }
        }
    }
}
