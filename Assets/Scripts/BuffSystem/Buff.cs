using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Buff : MonoBehaviour
{

    [SerializeField] private Image icon;
    [SerializeField] private BuffSO buff;

    [SerializeField] private TextMeshProUGUI buffText;

    [SerializeField] private bool mini = false;


    private float timer = 0f;

    private CharacterStats cS;

    private void Start()
    {
        cS = GetComponent<CharacterStats>();
    }

    private void Update()
    {
        if (buff)
        {
            if (TimeUp() && !cS)
            {
                // Deactivate.
                if (!mini) BuffPanel.Instance.RemoveBuff(this);
                else BuffPanel.TargetPanel.RemoveBuff(this);
            }
            else if (TimeUp() && cS)
            {
                // Deactivate.
                cS.RemoveBuff(buff);
            }
        }
    }

    public BuffSO GetBuff() { return buff; }

    public void SetBuff(Buff _buff)
    {
        buff = _buff.GetBuff();

        timer =_buff.timer;

        if (icon) icon.sprite = buff.buffImage;
    }

    public void SetBuff(Buff _buff, float time)
    {
        buff = _buff.GetBuff();

        timer = time;

        if (icon) icon.sprite = buff.buffImage;
    }

    public void SetBuff(BuffSO _buff)
    {
        buff = _buff;

        timer = buff.timeDur;

        if (icon) icon.sprite = buff.buffImage;
    }

    public void SetBuff(BuffSO _buff, float time)
    {
        buff = _buff;

        timer = time;

        if (icon) icon.sprite = buff.buffImage;
    }

    public void ReadBuff()
    {
        if (BuffTTReader.Instance) BuffTTReader.Instance.WriteToToolTip(this);
    }

    public void ReadMiniBuff()
    {
        if (BuffTTReader.TargetInstance) BuffTTReader.TargetInstance.WriteToToolTip(this);
    }

    public bool TimeUp()
    {
        if (buff.timeDur == -1) return false;

        timer -= Time.deltaTime;

        if (buffText)
        {
            float displayTimer = timer;

            if (displayTimer > 60)
            {
                displayTimer /= 60f;
                buffText.text = ((int)displayTimer).ToString() + "m";
            }
            else
            {
                buffText.text = ((int)displayTimer).ToString() + "s";
            }

        }

        if (timer <= 0)
        {
            return true;
        }

        return false;
    }

    public void Remove()
    {
        Destroy(this);
    }

    public void IncreaseStack()
    {
    }

    public void SetTimer(float t)
    {
        timer = t;
    }

    public float GetTimer()
    {
        return timer;
    }
}
