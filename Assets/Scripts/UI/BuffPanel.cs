using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffPanel : MonoBehaviour
{
    public static BuffPanel Instance { set; get; }
    public static BuffPanel TargetPanel { set; get; }

    [SerializeField] private GameObject buffPrefab;

    [SerializeField] private List<GameObject> buffPanel = new List<GameObject>();

    [SerializeField] private RectTransform startRect;

    [SerializeField] private CharacterPanel characterPanel;

    [SerializeField] private bool left;

    private void Awake()
    {
        if (!characterPanel && Instance == null) Instance = this;
        else if (characterPanel && TargetPanel == null) TargetPanel = this;
        else if (!characterPanel) Destroy(gameObject);
    }

    public void AddBuff(BuffSO buff)
    {
        int index = FindIndexOfBuff(buff);

        if (index != -1)
        {
            // Stack

            if (buffPanel[index].TryGetComponent<Buff>(out Buff b))
            {
                b.IncreaseStack();
            }    
        }
        else
        {
            GameObject go = Instantiate(buffPrefab, startRect.transform);

            buffPanel.Add(go);

            if (go.TryGetComponent<Buff>(out Buff buffGO))
            {
                buffGO.SetBuff(buff);
            }

            ReorderBuffs();
        }
    }

    public void AddBuff(Buff buff)
    {
        int index = FindIndexOfBuff(buff.GetBuff());

        if (index != -1)
        {
            // Stack

            if (buffPanel[index].TryGetComponent<Buff>(out Buff b))
            {
                b.IncreaseStack();
            }
        }
        else
        {
            GameObject go = Instantiate(buffPrefab, startRect.transform);

            buffPanel.Add(go);
            // 60 over 800 width
            if (go.TryGetComponent<Buff>(out Buff buffGO))
            {
                buffGO.SetBuff(buff);
            }

            ReorderBuffs();
        }
    }

    public void RemoveBuff(BuffSO buff)
    {
        int index = FindIndexOfBuff(buff);

        RemoveBuff(index);
    }

    public void RemoveBuff(int index)
    {
        if (index >= 0 && index < buffPanel.Count && buffPanel.Count > 0)
        {
            Buff buff = buffPanel[index].GetComponent<Buff>();

            if (!buff) return;

            RemoveBuff(buff);

        }
    }

    public void RemoveBuff(Buff buff)
    {
        if (buffPanel.Contains(buff.gameObject))
        {
            buffPanel.Remove(buff.gameObject);

            Destroy(buff.gameObject);

            ReorderBuffs();
        }
    }

    public void ReorderBuffs()
    {
        if (buffPanel.Count <= 0) return;

        if (!characterPanel)
        {
            for (int i = 0; i < buffPanel.Count; i++)
            {
                if (buffPanel[i].TryGetComponent<Image>(out Image buff))
                {
                    Vector2 rectPos = Vector2.zero;

                    float scaledValue = buff.rectTransform.rect.size.x / 800;

                    float scaledMoveDis = ((Camera.main.pixelWidth * scaledValue) * 0.80f);

                    if (!left) rectPos = new Vector2(startRect.position.x - (scaledMoveDis * i), buff.rectTransform.position.y);
                    else rectPos = new Vector2(startRect.position.x + (scaledMoveDis * i), buff.rectTransform.position.y);

                    buff.rectTransform.position = rectPos;
                }
            }
        }
        else
        {
            ReorderMiniBuffs();

            if (characterPanel)
            {
                characterPanel.AddToBuffPanel();
            }
        }
    }

    public void ReorderMiniBuffs()
    {
        if (buffPanel.Count <= 0) return;

        for (int i = 0; i < buffPanel.Count; i++)
        {
            if (buffPanel[i].TryGetComponent<Image>(out Image buff))
            {
                Vector2 rectPos = Vector2.zero;

                float scaledValue = buff.rectTransform.rect.size.x / 800;

                float scaledMoveDis = ((Camera.main.pixelWidth * scaledValue) * 0.6f);

                if (!left) rectPos = new Vector2(startRect.position.x - (scaledMoveDis * i), buff.rectTransform.position.y);
                else rectPos = new Vector2(startRect.position.x + (scaledMoveDis * i), buff.rectTransform.position.y);

                buff.rectTransform.position = rectPos;
            }
        }
    }

    public void RemoveAllFromPanel()
    {
        if (characterPanel)
        {
            foreach (GameObject go in buffPanel)
            {
                Destroy(go);
            }

            buffPanel.Clear();
        }
    }

    public void Reenable()
    {
        CharacterStats characterStats = characterPanel.GetCharacterStats();


        for (int i = 0; i < buffPanel.Count; i++)
        {
            if (buffPanel[i].TryGetComponent<Buff>(out Buff buff))
            {
                if (characterStats)
                {
                    Buff b = characterStats.GetCharacterVersion(buff);

                    if (b != null)
                    {
                        buff.SetTimer(b.GetTimer());                      
                    }
                    else
                    {
                        RemoveBuff(buff);
                    }
                }
            }
        }

    }

    public int FindIndexOfBuff(BuffSO buff)
    {
        for (int i = 0; i < buffPanel.Count; i++)
        {
            Buff b = buffPanel[i].GetComponent<Buff>();

            if (b)
            {
                if (b.GetBuff() == buff)
                {
                    return i;
                }
            }
        }

        return -1;
    }

}
