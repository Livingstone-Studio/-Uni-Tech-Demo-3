using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;


public class AbilityButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField] private UnityEvent onPress;

    private float abilityCooldownOne = -1f;
    private float abilityCooldownTwo = -1f;

    private Button button;

    [SerializeField] private TextMeshProUGUI cooldownText;
    public Image icon;

    [SerializeField] private float timeToHold = 1f;
    private float timer;

    private bool holding = false;

    private AbilitySO currentAb;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        AbilityCooldown();

        if (holding)
        {
            timer += Time.deltaTime;

            if (timer >= timeToHold)
            {
                OnHold();
            }
        }
    }

    public void Setup(AbilitySO ab)
    {
        currentAb = ab;

        // set other stuff

        if (icon) icon.sprite = ab.icon;
    }

    private void AbilityCooldown()
    {
        if (abilityCooldownOne > 0)
        {
            if (button.interactable && RogueAbilities.Instance.currentSetIndex == 0) SetButton(false);

            abilityCooldownOne -= Time.deltaTime;

            if (cooldownText && RogueAbilities.Instance.currentSetIndex == 0)
            {
                if (!cooldownText.enabled) cooldownText.enabled = true;

                cooldownText.text = ((int)abilityCooldownOne).ToString();
            }
        }
        
        if (abilityCooldownTwo > 0)
        {
            if (button.interactable && RogueAbilities.Instance.currentSetIndex == 1) SetButton(false);

            abilityCooldownTwo -= Time.deltaTime;

            if (cooldownText && RogueAbilities.Instance.currentSetIndex == 1)
            {
                if (!cooldownText.enabled) cooldownText.enabled = true;

                cooldownText.text = ((int)abilityCooldownTwo).ToString();
            }
        }

        if ((abilityCooldownOne < 0 && RogueAbilities.Instance.currentSetIndex == 0) || (abilityCooldownTwo < 0 && RogueAbilities.Instance.currentSetIndex == 1))
        {
            if (!button.interactable) SetButton(true);
            if (cooldownText.enabled) cooldownText.enabled = false;
        }
    }

    public void ButtonPress()
    {
        if (abilityCooldownOne > 0 || holding) return;

        onPress.Invoke();
    }

    public void StartCooldownOne(float cooldown)
    {   
        if (cooldown > 0 && abilityCooldownOne <= 0) abilityCooldownOne = cooldown;
        else RogueAbilities.Instance.GlobalCooldown(cooldown);
    }

    public void StartCooldownTwo(float cooldown)
    {
        if (cooldown > 0 && abilityCooldownTwo <= 0) abilityCooldownTwo = cooldown;
        else RogueAbilities.Instance.GlobalCooldown(cooldown);
    }


    public void SetButton(bool state)
    {
        button.enabled = state;
        if (state) icon.color = Color.white;
        else icon.color = Color.grey;
    }

    public void Toggle()
    {
        button.interactable = !button.interactable;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(DelayedStart());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //StartCoroutine(DelayedRelease());
        holding = false;

        if (AbilityTTReader.Instance && currentAb) AbilityTTReader.Instance.TurnOffTT();

    }

    public void OnHold()
    {
        if (AbilityTTReader.Instance && currentAb) AbilityTTReader.Instance.WriteToToolTip(currentAb);
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.6f);

        holding = true;
        timer = 0;
    }

    private IEnumerator DelayedRelease()
    {
        yield return new WaitForSeconds(0.1f);

        holding = false;

        if (AbilityTTReader.Instance && currentAb) AbilityTTReader.Instance.TurnOffTT();

    }

    public AbilitySO GetAbSO()
    {
        return currentAb;
    }

}
