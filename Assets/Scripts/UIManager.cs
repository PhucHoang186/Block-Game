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
        commandHolder.InitActions(OnMouseEnterCommandStack, onMouseExitCommandStack, UpdateACtionInfo);
        actionUIMover.InitAction(OnMouseRelease);
        commandStacker.InitAction(OnExecuteOneCommand);
        ActionButton.onMouseDownCb += ClickedOnActionButton;
    }

    void OnDestroy()
    {
        ActionButton.onMouseDownCb -= ClickedOnActionButton;
    }

    private void OnMouseEnterCommandStack()
    {
        if (actionUIMover.IsUsed)
        {
            isInRange = true;
            commandHolder.TogglePlaceHolderIcon(true);
        }
    }

    private void onMouseExitCommandStack()
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