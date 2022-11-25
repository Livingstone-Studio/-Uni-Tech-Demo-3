using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Panel { PLAYER, TARGET, TARGETTARGET}
public class CharacterPanel : MonoBehaviour
{
    public static CharacterPanel Player { set; get; }
    public static CharacterPanel Target { set; get; }
    public static CharacterPanel TargetTarget { set; get; }

    [SerializeField] private Panel panel;

    [SerializeField] private Image portrait;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image healthSliderIMG;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private Slider resourceSlider;
    [SerializeField] private Image resourceSliderIMG;
    [SerializeField] private TextMeshProUGUI resourceText;

    [SerializeField] private BuffPanel targetPanel;

    [SerializeField] private TextMeshProUGUI cPText;

    private CharacterStats characterStats;

    private Buff[] buffs;

    private void Awake()
    {

        if (panel == Panel.PLAYER) Player = this;
        if (panel == Panel.TARGET) Target = this;
        if (panel == Panel.TARGETTARGET) TargetTarget = this;

    }

    private void Start()
    {
        if (this != Player)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!characterStats) return;

        SetHealth();
        SetResource();
        EditCPText(characterStats.GetCurrentCPAmount());
    }

    public void Setup(CharacterStats cs)
    {
        if (!cs) return;

        gameObject.SetActive(true);

        characterStats = cs;

        resourceSlider.gameObject.SetActive(true);

        portrait.sprite = characterStats.GetPortrait();

        SetHealth();
        SetResource();

        if (targetPanel && panel == Panel.TARGET)
        {
            AddToBuffPanel();
        }

        switch (characterStats.GetResourceType())
        {
            case ResourceType.MANA:
                resourceSliderIMG.color = Color.blue;
                break;
            case ResourceType.RAGE:
                resourceSliderIMG.color = Color.red;
                break;
            case ResourceType.ENERGY:
                resourceSliderIMG.color = Color.yellow;
                break;
            case ResourceType.NONE:
                resourceSlider.gameObject.SetActive(false);
                break;
        }
    }

    public void SetHealth()
    {
        if (characterStats == null) return;

        healthText.text = (int)characterStats.GetHealth() + "/" + (int)characterStats.GetMaxHealth();

        healthSlider.value = NormalizedValue(characterStats.GetHealth(), characterStats.GetMaxHealth());
    }

    public void SetResource()
    {
        if (characterStats == null) return;

        resourceText.text = (int)characterStats.GetResource() + "/" + (int)characterStats.GetMaxResource();

        resourceSlider.value = NormalizedValue(characterStats.GetResource(), characterStats.GetMaxResource());
    }

    private float NormalizedValue(float value, float maxValue)
    {
        return value / maxValue;
    }

    public void AddToBuffPanel()
    {

        buffs = characterStats.GetAllBuffs();

        foreach (Buff be in buffs)
        {
            targetPanel.AddBuff(be);
        }

    }

    public CharacterStats GetCharacterStats()
    {
        return characterStats;
    }

    public void EditCPText(int amount)
    {
        if (!cPText) return;

        cPText.text = amount.ToString() + " combo points";
    }

    private void OnEnable()
    {
        if (targetPanel) targetPanel.Reenable();
    }
}
