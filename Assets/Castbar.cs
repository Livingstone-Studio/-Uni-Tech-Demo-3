using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castbar : MonoBehaviour
{
    public static Slider Instance { set; get; }

    private void Awake()
    {
        if (Instance == null) Instance = GetComponent<Slider>();
    }
}
