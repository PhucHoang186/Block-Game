using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandListDisplay : MonoBehaviour
{
    [SerializeField] ActionButton actionButtonPrefab;
    [SerializeField] RectTransform panel;
    [SerializeField] float topBottomOffset;
    [SerializeField] List<ActionData> actionDataList;
    [SerializeField] List<ActionData> tutorialActionDataList;
    private List<ActionData> actionList;
    private List<ActionButton> actionButtonList = new();

    private void GenerateCommandList()
    {
        actionList = GameDataManager.Instance.IsTutorialLevel ? tutorialActionDataList : actionDataList;
        foreach (var actionData in actionList)
        {
            var actionButton = Instantiate(actionButtonPrefab, panel.transform);
            actionButton.Init(actionData);
            actionButtonList.Add(actionButton);
        }
    }


    public Sprite GetActionButtonImage(ActionType actionType)
    {
        foreach (var actionData in actionDataList)
        {
            if (actionData.actionType == actionType)
                return actionData.actionIcon;
        }
        return null;
    }

    public Vector3 GetCommandButtonPosition(ActionType actionType)
    {
        foreach (var button in actionButtonList)
        {
            if (button.CompareActionType(actionType))
                return button.transform.position;
        }
        return Vector3.zero;
    }

    private void ReleaseCommandButton()
    {
        for (int i = 0; i < panel.childCount; i++)
        {
            Destroy(panel.GetChild(i).gameObject);
        }
    }

    private void CalculateContentSize()
    {
        float height = 300f * actionList.Count + topBottomOffset * 2f;
        panel.sizeDelta = new Vector2(panel.sizeDelta.x, height);
    }

    public void EnableCommandList()
    {
        ReleaseCommandButton();
        GenerateCommandList();
        CalculateContentSize();
        ToggleCommandList(true);
    }

    public void DisableCommandList()
    {
        ToggleCommandList(false);
    }

    private void ToggleCommandList(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

}
