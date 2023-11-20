using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject placeHolderObj;
    [SerializeField] Image placeHolderIcon;
    [SerializeField] GameObject commandExcuteIcon;
    [SerializeField] ActionUIDisplay actionDisplayPrefab;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] Transform content;
    [SerializeField] List<GameObject> dummyIcons;
    [SerializeField] ScrollContentButton leftScrollButton;
    [SerializeField] ScrollContentButton rightScrollButton;
    [SerializeField] GameObject blockInputObj;
    [SerializeField] GameObject executeButton;
    [SerializeField] GameObject deleteButton;
    [SerializeField] float scrollSpeed;
    private Action onMouseEnterCommandStack;
    private Action onMouseExitCommandStack;
    private Action<int, int> onUpdateActionInfo;
    private int executeIndex;
    private bool isAutoScroll;
    List<ActionUIDisplay> UIDisplayList = new();

    void Start()
    {
        leftScrollButton.InitAction(ScrollContent);
        rightScrollButton.InitAction(ScrollContent);
    }

    public void InitActions(Action mouseEnter, Action mouseExit, Action<int, int> updateActionInfo)
    {
        onMouseEnterCommandStack = mouseEnter;
        onMouseExitCommandStack = mouseExit;
        onUpdateActionInfo = updateActionInfo;
    }

    public void UpdateIcon(Sprite actionIcon)
    {
        placeHolderIcon.sprite = actionIcon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onMouseEnterCommandStack?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onMouseExitCommandStack?.Invoke();
    }

    public void UpdateCommandExecuteIcon()
    {
        ToggleExecuteIcon(true);
        // commandExcuteIcon.transform.position = UIDisplayList[executeIndex].transform.position;
        executeIndex++;
        executeIndex = Mathf.Clamp(executeIndex, 0, UIDisplayList.Count);
    }

    public void ResetExecuteList()
    {
        executeIndex = 0;
        ToggleExecuteIcon(false);
    }

    public void AddActionUI(ActionData actionData)
    {
        var actionUI = Instantiate(actionDisplayPrefab, content);
        actionUI.UpdateIcon(actionData.actionIcon);
        actionUI.InitAction(UpdateActionInfo);
        UIDisplayList.Add(actionUI);
        if (UIDisplayList.Count <= 1)
        {
            ToggleCommandButtons(true);
        }
    }

    private void ToggleCommandButtons(bool isActive)
    {
        executeButton.SetActive(isActive);
        deleteButton.SetActive(isActive);
    }

    public void ResetCommands()
    {
        ToggleCommandButtons(false);
        foreach (var actionUI in UIDisplayList)
        {
            Destroy(actionUI.gameObject);
        }
        UIDisplayList.Clear();
    }

    private void AutoScrollToEnd()
    {
        ScrollContent(1);
    }

    public void ScrollContent(int direction)
    {
        scrollRect.horizontalNormalizedPosition += scrollSpeed * direction;
    }

    public void TogglePlaceHolderIcon(bool isActive)
    {
        placeHolderObj.SetActive(isActive && placeHolderIcon.sprite != null);
        isAutoScroll = isActive;
        if (isActive)
        {
            placeHolderObj.transform.SetAsLastSibling();
        }
        foreach (var dummy in dummyIcons)
            dummy.transform.SetAsLastSibling();
    }

    private void UpdateActionInfo(ActionUIDisplay actionUIDisplay)
    {
        int index = UIDisplayList.IndexOf(actionUIDisplay);
        if (index >= UIDisplayList.Count)
            return;
        onUpdateActionInfo?.Invoke(index, actionUIDisplay.ActionCount);
    }

    public void ToggleExecuteIcon(bool isActive)
    {
        commandExcuteIcon.SetActive(isActive);
    }

    void Update()
    {
        if (!isAutoScroll)
            return;
        AutoScrollToEnd();
    }

    public void BlockInteract()
    {
        ToggleCommandButtons(false);
        ToggleBlockInput(true);
    }

    public void UnBlockInteract()
    {
        ToggleCommandButtons(true);
        ToggleBlockInput(false);
    }

    public void ToggleBlockInput(bool isActive)
    {
        blockInputObj.SetActive(isActive);
    }
}
