using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] GameObject mainUI;
    [SerializeField] CommandStacker commandStacker;
    [SerializeField] ActionUIMover actionUIMover;
    [SerializeField] CommandHolder commandHolder;
    [SerializeField] CommandListDisplay commandList;
    [SerializeField] TutorialPanel tutorialPanel;
    [SerializeField] GameObject blockInput;
    ActionData currentAction;
    private bool isInRange;
    public bool IsBlockInput => blockInput.activeSelf;

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

    public void OnBlockInput(bool isActive)
    {
        blockInput.SetActive(isActive);
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

    public void ToggleMainUI(bool isActive)
    {
        mainUI.SetActive(isActive);
    }


    public void OnShowHandDragAction(ActionType actionType)
    {
        tutorialPanel.ShowHandTutorialDragAction(commandList.GetCommandButtonPosition(actionType), commandHolder.GetTutorialCommandPosition(), commandList.GetActionButtonImage(actionType));
    }

    public void OnShowHandTapDemoButton()
    {
        tutorialPanel.ShowHandTapTutorialAction();
    }

    public void ShowDialogText(string content)
    {
        ToggleMainUI(false);
        tutorialPanel.ToggleDialogText(true, content);
    }

    public void HideDialogText()
    {
        ToggleMainUI(true);
        tutorialPanel.ToggleDialogText(false);
    }

    private void UpdateACtionInfo(int index, int actionCount)
    {
        commandStacker.UpdateActionInfo(index, actionCount);
    }

    public void ShowIntroPanel()
    {
        tutorialPanel.ToggleIntroPanel(true);
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
        commandHolder.ResetCommand();
        commandStacker.ResetCommands();
    }

    public void RunCommands()
    {
        commandList.DisableCommandList();
        commandHolder.RunCommand();
        commandStacker.ExecuteCommands();
    }

    private void ClickedOnActionButton(ActionData actionData)
    {
        currentAction = actionData;
        actionUIMover.OnStartMoving(actionData.actionIcon);
        commandHolder.UpdateIcon(currentAction.actionIcon);
    }

    public void ShowRotateButtonTutorial()
    {
        tutorialPanel.ToggleButtonTutorial(true);
        DOVirtual.DelayedCall(1.5f, OnShowHandTapDemoButton);
    }

    public void OnBackButtonPress()
    {
        GameManager.Instance.LoadLevelSelectScene();
    }

    public void OnShowTutorialButtonPress()
    {
        tutorialPanel.ToggleVideoTutorial(true);
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
