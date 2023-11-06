using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandListDisplay : MonoBehaviour
{
    [SerializeField] ActionButton actionButtonPrefab;
    [SerializeField] RectTransform panel;
    [SerializeField] float topBottomOffset;
    [SerializeField] List<ActionData> actionDataList;

    void Start()
    {
        CalculateContentSize();
        GenerateCommandList();
    }

    private void GenerateCommandList()
    {
        foreach (var actionData in actionDataList)
        {
            var actionButton = Instantiate(actionButtonPrefab, panel.transform);
            actionButton.Init(actionData);
        }
    }

    private void CalculateContentSize()
    {
        float height = 300f * actionDataList.Count + topBottomOffset * 2f;
        panel.sizeDelta = new Vector2(panel.sizeDelta.x, height);
    }

    public void EnableCommandList()
    {
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
