using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffTTReader : MonoBehaviour
{
    public static BuffTTReader Instance { set; get; }
    public static BuffTTReader TargetInstance { set; get; }

    [SerializeField] private GameObject buffTTPanel;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descText;

    [SerializeField] private GameObject removeButton;

    private Buff currentBuff;

    [SerializeField] private bool targetTT = false;

    private void Awake()
    {
        if (Instance == null && !targetTT)
        {
            Instance = this;
        }
        else if (TargetInstance == null && targetTT)
        {
            TargetInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (buffTTPanel) buffTTPanel.SetActive(false);
    }

    public void WriteToToolTip(Buff buff)
    {
        if (buffTTPanel) buffTTPanel.SetActive(true);
     
        currentBuff = null;

        if (removeButton)
        {
            if (buff.GetBuff().buffType == BuffType.BUFF)
            {
                removeButton.SetActive(true);
            }
            else
            {
                removeButton.SetActive(false);
            }
        }

        if (titleText) titleText.text = buff.GetBuff().title;
        if (descText) descText.text = buff.GetBuff().desc;


        currentBuff = buff;
    }

    public void DismissFromToolTip()
    {
        if (titleText) titleText.text = "";
        if (descText) descText.text = "";

        if (buffTTPanel) buffTTPanel.SetActive(false);

        currentBuff = null;
    }

    public void RemoveBuff()
    {
        if (CharacterStats.Player && currentBuff) CharacterStats.Player.RemoveBuff(currentBuff.GetBuff());

        DismissFromToolTip();
    }

    private IEnumerator TempDisplay(BuffSO buff)
    {
        if (buffTTPanel) buffTTPanel.SetActive(true);

        if (titleText) titleText.text = buff.title;
        if (descText) descText.text = buff.desc;

        yield return new WaitForSeconds(2f);

        if (titleText) titleText.text = "";
        if (descText) descText.text = "";


        if (buffTTPanel) buffTTPanel.SetActive(false);
    }
}
