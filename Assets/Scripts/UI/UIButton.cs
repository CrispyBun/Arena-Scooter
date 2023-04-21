using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class UIButton : MonoBehaviour
{
    private RectTransform buttonTransform;
    private float buttonWidth;
    private float buttonHeight;

    private GameObject buttonContent;
    private TextMeshProUGUI buttonText;
    private float buttonTextSize;
    private float buttonTextX;
    private float buttonTextY;

    private UnityEngine.UI.Image selectButtonLeft;
    private UnityEngine.UI.Image selectButtonRight;

    private float buttonSizeChange = 40;
    private float fontSizeChange = 5;
    private float fontShakeAmount = 5;

    [SerializeField] private bool selected = false;
    private bool disabledFrame = false;

    [SerializeField] protected GameObject navigateLeft;
    [SerializeField] protected GameObject navigateRight;
    [SerializeField] protected GameObject navigateUp;
    [SerializeField] protected GameObject navigateDown;

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

        selectButtonLeft = transform.GetChild(1).gameObject.GetComponent<UnityEngine.UI.Image>();
        selectButtonRight = transform.GetChild(2).gameObject.GetComponent<UnityEngine.UI.Image>();
        selectButtonLeft.color = new Color(0, 0, 0, 0);
        selectButtonRight.color = new Color(0, 0, 0, 0);
    }

    void FixedUpdate()
    {
        if (IsSelected())
        {
            // Selection Animation
            float fontOffsetX = Random.Range(-fontShakeAmount, fontShakeAmount);
            float fontOffsetY = Random.Range(-fontShakeAmount, fontShakeAmount);

            buttonContent.transform.localPosition = new Vector3(buttonTextX + fontOffsetX, buttonTextY + fontOffsetY, buttonContent.transform.localPosition.z);

            buttonTransform.sizeDelta = new Vector2(buttonWidth - buttonSizeChange, buttonHeight);
            buttonText.fontSize = buttonTextSize + fontSizeChange;

            selectButtonLeft.color = Color.white;
            selectButtonRight.color = Color.white;
        }
        else
        {
            buttonTransform.sizeDelta = new Vector2(buttonWidth, buttonHeight);
            buttonText.fontSize = buttonTextSize;

            selectButtonLeft.color = new Color(0, 0, 0, 0);
            selectButtonRight.color = new Color(0, 0, 0, 0);
        }
    }

    void Update()
    {
        if (IsSelected())
        {
            if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.Space))
            {
                // ui sfx go here later
                OnButtonClick();
            }

            // mm yummy code
            // button navigation
            if (navigateLeft && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)))
            {
                SwitchNavigation(navigateLeft);
            }
            else if (navigateRight && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)))
            {
                SwitchNavigation(navigateRight);
            }
            else if (navigateUp && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
            {
                SwitchNavigation(navigateUp);
            }
            else if (navigateDown && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
            {
                SwitchNavigation(navigateDown);
            }
        }

        disabledFrame = false;
    }

    protected virtual void OnButtonClick()
    {

    }

    public void SelectButton()
    {
        selected = true;
        disabledFrame = true;
    }

    public bool IsSelected()
    {
        return selected && !disabledFrame;
    }

    private void SwitchNavigation(GameObject button)
    {
        UIButton switchingButtonClass = button.GetComponent<UIButton>();
        selected = false;
        switchingButtonClass.SelectButton();
    }
}
