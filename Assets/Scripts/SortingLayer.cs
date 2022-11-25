using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sR;

    private int startingLayer = 0;

    private void Start()
    {
        if (sR) startingLayer = sR.sortingOrder;
    }

    private void Update()
    {
        if (sR) sR.sortingOrder = startingLayer - (int)transform.position.y;
    }
}
