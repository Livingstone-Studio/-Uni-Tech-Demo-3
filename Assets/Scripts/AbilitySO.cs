using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/Abilities ")]
public class AbilitySO : ScriptableObject
{
    public Sprite icon;

    public string title;
    public string desc;

    public float cooldown;
    public float cost;
}
