using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackButton : MonoBehaviour
{
    public UnityEvent onDisable;

    private void OnDisable()
    {
        onDisable.Invoke();
    }
}