using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public enum TutorialStep
{
    Show_Objective,
    Show_Video_Tutorial,
    Show_UI_Tutorial,

}

public enum CamType
{
    Game_Cam,
    Player_Cam,
    Objective_Cam
}

public class TutorialController : MonoBehaviour
{
    public static TutorialController Instance;
    public const float CAMERA_TRANSITON_TIME = 1f;
    [Header("Reference Use For Tutorials")]
    [SerializeField] CinemachineVirtualCamera normalCam;
    [SerializeField] CinemachineVirtualCamera playerFocusCam;
    [SerializeField] CinemachineVirtualCamera objecttiveFocusCam;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    void OnDestroy()
    {
        Instance = null;
    }

    public void OnChangeTutorialState(TutorialStep tutorialStep)
    {
        switch (tutorialStep)
        {
            case TutorialStep.Show_Objective:
                ShowObjectives();
                break;
            case TutorialStep.Show_Video_Tutorial:
                ShowtutorialVideo();
                break;
        }
    }

    private void ShowtutorialVideo()
    {
        UIManager.Instance.OnShowTutorialButtonPress();
    }

    private void ShowObjectives()
    {
        StartCoroutine(CorShowObjective());
    }

    private IEnumerator CorShowObjective()
    {
        // focus on player
        UIManager.Instance.ToggleMainUI(false);
        yield return new WaitForSeconds(1f);
        PlayerCharacter player = GameManager.Instance.GetPlayerInLevel();
        SetCamFocus(playerFocusCam, player.transform);
        yield return new WaitForSeconds(0.1f);
        SetCamPriority(CamType.Player_Cam);
        yield return new WaitForSeconds(CAMERA_TRANSITON_TIME);
        UIManager.Instance.ShowDialogText("Help this cute little duck...");
        yield return new WaitForSeconds(2.5f);

        // focus on objective
        GameObject objective = GameManager.Instance.GetObjectiveInLevel();
        SetCamFocus(objecttiveFocusCam, objective.transform);
        SetCamPriority(CamType.Objective_Cam);
        yield return new WaitForSeconds(CAMERA_TRANSITON_TIME);
        UIManager.Instance.ShowDialogText("...reach this flag to win!");
        yield return new WaitForSeconds(2.5f);

        // return to game cam
        UIManager.Instance.HideDialogText();
        SetCamPriority(CamType.Game_Cam);

        yield return new WaitForSeconds(1f);
        OnChangeTutorialState(TutorialStep.Show_Video_Tutorial);
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

}
