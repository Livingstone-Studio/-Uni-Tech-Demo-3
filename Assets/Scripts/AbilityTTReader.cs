using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityTTReader : MonoBehaviour
{
    public static AbilityTTReader Instance { set; get; }

    [SerializeField] private GameObject abilityTTPanel;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (abilityTTPanel) abilityTTPanel.SetActive(false);
    }

    public void WriteToToolTip(AbilitySO ability)
    {
        if (abilityTTPanel) abilityTTPanel.SetActive(true);

        if (titleText) titleText.text = ability.title;
        if (descText) descText.text = ability.desc;
    }

    public void TurnOffTT()
    {
        if (abilityTTPanel.activeInHierarchy) abilityTTPanel.SetActive(false);
    }
}
