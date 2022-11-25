using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DmgText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dmgText;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float moveMultiplier = 100f;

    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * moveSpeed * moveMultiplier;
    }

    public void SetText(int dmg, Color colour)
    {
        dmgText.text = dmg.ToString();
        dmgText.color = colour;
    }
}
