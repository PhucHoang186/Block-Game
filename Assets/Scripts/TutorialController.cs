using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public enum TutorialStep
{
    Show_Intro,
    Show_Objective,
    Show_Video_Tutorial,
    Show_Hand_Drag_Action,
    Show_Button_Tutorial,

}

public enum CamType
{
    Game_Cam,
    Player_Cam,
    Objective_Cam
}

public class TutorialController : MonoBehaviour
{
    public static Action ON_CLOSE_TUTORIAL_PANEL;
    public static TutorialController Instance;
    public const float CAMERA_TRANSITON_TIME = 1f;
    [Header("Reference Use For Tutorials")]
    [SerializeField] CinemachineVirtualCamera normalCam;
    [SerializeField] CinemachineVirtualCamera playerFocusCam;
    [SerializeField] CinemachineVirtualCamera objecttiveFocusCam;
    private bool isTutorialPhase;
    TutorialStep tutorialStep;
    private Queue<SequenceEvent> tutorialSequence = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        ON_CLOSE_TUTORIAL_PANEL += OnCloseTutorialPanel;
    }

    void OnDestroy()
    {
        ON_CLOSE_TUTORIAL_PANEL -= OnCloseTutorialPanel;
        Instance = null;
    }

    public void OnChangeTutorialState(TutorialStep tutorialStep)
    {
        this.tutorialStep = tutorialStep;
        switch (tutorialStep)
        {
            case TutorialStep.Show_Intro:
                ShowIntro();
                break;
            case TutorialStep.Show_Objective:
                ShowObjectives();
                break;
            case TutorialStep.Show_Video_Tutorial:
                ShwoTutorialVideo();
                break;
            case TutorialStep.Show_Hand_Drag_Action:
                ShowHandDragAction();
                break;
            case TutorialStep.Show_Button_Tutorial:
                ShowRotateButton();
                break;
        }
    }

    private void OnCloseTutorialPanel()
    {
        StartCoroutine(CorOnCloseTutorialPanel());
    }

    private IEnumerator CorOnCloseTutorialPanel()
    {
        if (!isTutorialPhase)
            yield break;
        if (tutorialStep == TutorialStep.Show_Intro)
        {
            OnChangeTutorialState(TutorialStep.Show_Objective);
        }
        else
        {
            UIManager.Instance.OnBlockInput(true);
            OnChangeTutorialState(TutorialStep.Show_Hand_Drag_Action);
            yield return new WaitForSeconds(2f);
            UIManager.Instance.OnBlockInput(false);
        }
    }

    private void ShowIntro()
    {
        if (GameDataManager.Instance?.GetFTUEState() >= (int)FTUE_State.Intro_Level_0)
            return;
        GameDataManager.Instance?.SaveFTUEStep(FTUE_State.Intro_Level_0);
        isTutorialPhase = true;
        UIManager.Instance.ShowIntroPanel();
    }

    private void ShowHandDragAction()
    {
        isTutorialPhase = false;
        UIManager.Instance.OnShowHandDragAction(GameDataManager.Instance.GetLevelActionButtonType());
    }

    private void ShowRotateButton()
    {
        if (GameDataManager.Instance?.GetFTUEState() >= (int)FTUE_State.Intro_Level_1)
            return;
        GameDataManager.Instance?.SaveFTUEStep(FTUE_State.Intro_Level_1);
        isTutorialPhase = true;
        UIManager.Instance.ShowRotateButtonTutorial();
    }

    private void ShwoTutorialVideo()
    {
        UIManager.Instance.OnShowTutorialButtonPress();
    }


    private void ShowObjectives()
    {
        Action finishCallback = CallSequence;
        SequenceEvent showFirstPartDialog = new SequenceEvent(ShowFirstPartDialog, finishCallback);
        SequenceEvent showFinalPartDialog = new SequenceEvent(ShowFinalPartDialog, finishCallback);
        SequenceEvent backToTopView = new SequenceEvent(BackToTopView, finishCallback);
        tutorialSequence.Enqueue(showFirstPartDialog);
        tutorialSequence.Enqueue(showFinalPartDialog);
        tutorialSequence.Enqueue(backToTopView);
        CallSequence();
    }

    private void CallSequence()
    {
        if (tutorialSequence.Count > 0)
            tutorialSequence.Dequeue().PlaySequence();
    }

    private void ShowFirstPartDialog(Action finishCb)
    {
        StartCoroutine(CorShowFirstPartDialog(finishCb));
    }

    private IEnumerator CorShowFirstPartDialog(Action finishCb)
    {
        UIManager.Instance.ToggleMainUI(false);
        yield return new WaitForSeconds(1f);
        PlayerCharacter player = GameManager.Instance.GetPlayerInLevel();
        SetCamFocus(playerFocusCam, player.transform);
        SetCamPriority(CamType.Player_Cam);
        yield return new WaitForSeconds(CAMERA_TRANSITON_TIME);
        GameManager.Instance.ShowPlayerIntro();
        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowDialogText("Hãy giúp bạn vịt Bi dễ thương này...");
        finishCb?.Invoke();
    }

    private void ShowFinalPartDialog(Action finishCb)
    {
        StartCoroutine(CorShowFinalPartDialog(finishCb));
    }

    private IEnumerator CorShowFinalPartDialog(Action finishCb)
    {
        // focus on objective
        yield return new WaitForSeconds(2.5f);
        GameObject objective = GameManager.Instance.GetObjectiveInLevel();
        SetCamFocus(objecttiveFocusCam, objective.transform);
        SetCamPriority(CamType.Objective_Cam);
        yield return new WaitForSeconds(CAMERA_TRANSITON_TIME);
        GameManager.Instance.ShowEndPointIntro();
        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowDialogText("...đi đến lá cờ này để tìm đường về nhà!");
        finishCb?.Invoke();
    }

    private void BackToTopView(Action finishCb)
    {
        StartCoroutine(CorBackToToiopView(finishCb));
    }

    private IEnumerator CorBackToToiopView(Action finishCb)
    {
        yield return new WaitForSeconds(2.5f);
        // return to game cam
        UIManager.Instance.HideDialogText();
        SetCamPriority(CamType.Game_Cam);
        yield return new WaitForSeconds(1f);
        OnChangeTutorialState(TutorialStep.Show_Video_Tutorial);
        finishCb?.Invoke();
    }

    private void SetCamPriority(CamType camType)
    {
        CinemachineVirtualCamera cam;
        switch (camType)
        {
            case CamType.Game_Cam:
                cam = normalCam;
                break;
            case CamType.Player_Cam:
                cam = playerFocusCam;
                break;
            default:
                cam = objecttiveFocusCam;
                break;
        }

        // reset priority
        normalCam.Priority = 10;
        playerFocusCam.Priority = 10;
        objecttiveFocusCam.Priority = 10;

        cam.Priority = 99;
    }

    private void SetCamFocus(CinemachineVirtualCamera cam, Transform target)
    {
        cam.m_Follow = target;
        cam.m_LookAt = target;
    }


    public class SequenceEvent
    {
        public Action<Action> actionCb;
        public Action actionFinishCb;

        public SequenceEvent(Action<Action> actionCb, Action actionFinishCb)
        {
            this.actionCb = actionCb;
            this.actionFinishCb = actionFinishCb;
        }

        public void PlaySequence()
        {
            actionCb?.Invoke(actionFinishCb);
        }
    }
}
