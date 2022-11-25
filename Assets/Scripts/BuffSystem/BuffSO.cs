using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType { BUFF, DEBUFF }

[CreateAssetMenu(menuName = "My Assets/Buff ")]
public class BuffSO : ScriptableObject
{
    public string title = "";

    public string desc = "";

    public float timeDur = -1f; // seconds, -1 == infinite

    public Sprite buffImage;

    public BuffType buffType = BuffType.BUFF;

    public bool affectSpriteTranspancey = false;
    
    public bool affectAttackRate = false;
    public bool affectMovementRate = false;
    public bool affectResourceRate = false;
    public float newRate = 1f;

    public float newRate2 = 1f;

    public bool affectResourceAmount;

    public float newResourceMax;

    public bool characterPanic = false;

    public bool toxic = false;

}
