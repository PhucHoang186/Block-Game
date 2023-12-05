using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] CommandStacker commandStacker;
    [SerializeField] ActionUIMover actionUIMover;
    [SerializeField] CommandHolder commandHolder;
    [SerializeField] CommandListDisplay commandList;
    [SerializeField] Transform handtutorial;
    ActionData currentAction;
    private bool isInRange;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        commandHolder.InitActions(OnMouseEnterCommandStack, OnMouseExitCommandStack, UpdateACtionInfo);
        actionUIMover.InitAction(OnMouseRelease);
        commandStacker.InitAction(OnExecuteOneCommand, OnExecuteAllCommands);
        ActionButton.onMouseDownCb += ClickedOnActionButton;
    }

    void OnDestroy()
    {
        ActionButton.onMouseDownCb -= ClickedOnActionButton;
    }

    private void Update()
    {
        handtutorial.position = Input.mousePosition;
    }

    private void OnMouseEnterCommandStack()
    {
        if (actionUIMover.IsUsed)
        {
            isInRange = true;
            commandHolder.TogglePlaceHolderIcon(true);
        }
    }

    private void OnMouseExitCommandStack()
    {
        isInRange = false;
        commandHolder.TogglePlaceHolderIcon(false);
    }

    private void OnMouseRelease()
    {
        if (isInRange)
        {
            ActionInfo actionInfo = new(1, currentAction);
            commandStacker.AddActionInfo(actionInfo);
            commandHolder.AddActionUI(currentAction);
            commandHolder.TogglePlaceHolderIcon(false);
        }
    }


    private void UpdateACtionInfo(int index, int actionCount)
    {
        commandStacker.UpdateActionInfo(index, actionCount);
    }

    private void OnExecuteOneCommand()
    {
        commandHolder.UpdateCommandExecuteIcon();
        GameManager.Instance.CheckLoseCondition();
    }
    
    private void OnExecuteAllCommands()
    {
        GameManager.Instance.CheckWinCondition();
    }

    public void ResetCommands()
    {
        commandList.EnableCommandList();
        commandHolder.ResetCommands();
        commandHolder.UnBlockInteract();
        commandStacker.ResetCommands();
    }

    public void RunCommands()
    {
        commandList.DisableCommandList();
        commandHolder.ResetExecuteList();
        commandHolder.BlockInteract();
        commandStacker.ExecuteCommands();
    }

    private void ClickedOnActionButton(ActionData actionData)
    {
        currentAction = actionData;
        actionUIMover.OnStartMoving(actionData.actionIcon);
        commandHolder.UpdateIcon(currentAction.actionIcon);
    }

    public void OnBackButtonPress()
    {
        GameManager.Instance.LoadLevelSelectScene();
    }
}

[Serializable]
public class ActionInfo
{
    public ActionInfo(int actionCount, ActionData actionData)
    {
        this.actionCount = actionCount;
        this.actionData = actionData;
    }

    public int actionCount;
    public ActionData actionData;
}

public class UIEvents
{
    public static Action<bool> ON_TRANSITION_IN;
}
