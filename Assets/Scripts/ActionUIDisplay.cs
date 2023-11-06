using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionUIDisplay : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Image actionIcon;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] GameObject input;
    [SerializeField] TMP_Text actionCountText;
    private RectTransform rectTransform;
    private bool isSelectInput;
    private Action<ActionUIDisplay> onUpdateActionInfoCb;
    public int ActionCount { get; set; }

    void Start()
    {
        rectTransform = transform as RectTransform;
        ToggleInputField(true);
        inputField.characterLimit = 2;
        inputField.characterValidation = TMP_InputField.CharacterValidation.Integer;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isSelectInput)
            {
                isSelectInput = false;
                return;
            }

            if (!CheckIfTapOnThisUI())
            {
                ToggleInputField(false);
            }
        }
    }

    public void InitAction(Action<ActionUIDisplay> onUpdateActionInfo)
    {
        onUpdateActionInfoCb = onUpdateActionInfo;
    }

    public void UpdateIcon(Sprite sprite)
    {
        actionIcon.sprite = sprite;
    }

    public void ToggleInputField(bool isActive)
    {
        input.SetActive(isActive);
    }

    public void OnSelectInputField()
    {
        isSelectInput = true;
    }

    public void OnAssignActionCount()
    {
        int.TryParse(inputField.text, out int count);
        if (count <= 0)
        {
            count = 1;
        }
        ActionCount = count;
        actionCountText.text = count.ToString();
        onUpdateActionInfoCb?.Invoke(this);

    }

    private bool CheckIfTapOnThisUI()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ToggleInputField(true);
    }
}
