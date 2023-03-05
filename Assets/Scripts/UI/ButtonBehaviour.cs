using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class ButtonBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform buttonTransform;
    private float buttonWidth;
    private float buttonHeight;

    private GameObject buttonContent;
    private TextMeshProUGUI buttonText;
    private float buttonTextSize;
    private float buttonTextX;
    private float buttonTextY;

    private float buttonSizeChange = 60;
    private float fontSizeChange = 5;
    private float fontShakeAmount = 5;

    private bool doShake = false;

    void Start()
    {
        buttonTransform = GetComponent<RectTransform>();
        buttonWidth = buttonTransform.sizeDelta.x;
        buttonHeight = buttonTransform.sizeDelta.y;

        buttonContent = transform.GetChild(0).gameObject;
        buttonText = buttonContent.GetComponent<TextMeshProUGUI>();
        buttonTextSize = buttonText.fontSize;
        buttonTextX = buttonContent.transform.localPosition.x;
        buttonTextY = buttonContent.transform.localPosition.y;
    }

    void FixedUpdate()
    {
        if (doShake)
        {
            float fontOffsetX = Random.Range(-fontShakeAmount, fontShakeAmount);
            float fontOffsetY = Random.Range(-fontShakeAmount, fontShakeAmount);

            buttonContent.transform.localPosition = new Vector3(buttonTextX + fontOffsetX, buttonTextY + fontOffsetY, buttonContent.transform.localPosition.z);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonTransform.sizeDelta = new Vector2(buttonWidth - buttonSizeChange, buttonHeight);
        buttonText.fontSize = buttonTextSize + fontSizeChange;
        doShake = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonTransform.sizeDelta = new Vector2(buttonWidth, buttonHeight);
        buttonText.fontSize = buttonTextSize;
        doShake = false;
    }
}
