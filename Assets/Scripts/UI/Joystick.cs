using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    public static Joystick Instance { set; get; }

    private Image joystick;
    private Image joystickBKGND;

    private Vector2 inputPosition;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        joystickBKGND = GetComponent<Image>();
        joystick = transform.GetChild(0).GetComponent<Image>();
    }

    public Vector2 GetNormalizedMovement()
    {
        return (joystick.rectTransform.position - joystickBKGND.rectTransform.position).normalized;
    }

    #region Interface Methods

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBKGND.rectTransform, eventData.position, eventData.pressEventCamera, out inputPosition))
        {
            inputPosition = new Vector2(inputPosition.x / joystickBKGND.rectTransform.sizeDelta.x, inputPosition.y / joystickBKGND.rectTransform.sizeDelta.y);

            if (inputPosition.magnitude >= 1f)
            {
                inputPosition = inputPosition.normalized;
            }

            joystick.rectTransform.anchoredPosition = new Vector2(inputPosition.x * (joystickBKGND.rectTransform.sizeDelta.x / 3), inputPosition.y * (joystickBKGND.rectTransform.sizeDelta.y / 3));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystick.rectTransform.anchoredPosition = Vector2.zero;
        inputPosition = Vector2.zero;
    }

    #endregion
}
