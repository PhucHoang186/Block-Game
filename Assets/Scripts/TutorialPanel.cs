using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
using System.Text;
using TMPro;


public enum HandTutorialType
{
    Tap,
    Drag
}

public class TutorialPanel : MonoBehaviour
{
    [Header("Tutorial Video")]
    [SerializeField] GameObject videoPanel;
    [SerializeField] Transform panel;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] Button replayButton;

    [SerializeField] GameObject textHolder;
    [SerializeField] TMP_Text diaglogText;
    [Header("Tutorial Button")]

    [SerializeField] GameObject buttonPanel;
    [SerializeField] HandTutorialUI tutorialHand;
    [SerializeField] Image actionButtonImage;
    [SerializeField] GameObject playerRender;
    [SerializeField] List<DemoButton> demoButtons;
    [SerializeField] GameObject introPanel;

    private Vector3 startHandTutorial;
    private Vector3 endHandTutorial;
    private bool isShowHandTutorial;
    private bool isShowingHandTutorial;
    private HandTutorialType currentHandTutorialType;

    void Start()
    {
        InitDemoButtons();
        videoPlayer.loopPointReached += OnVideoEnd;
        replayButton.onClick.AddListener(PlayVideo);
    }


    void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
        replayButton.onClick.RemoveListener(PlayVideo);
    }

    private void InitDemoButtons()
    {
        for (int i = 0; i < demoButtons.Count; i++)
        {
            demoButtons[i].Init(playerRender);
        }
    }

    void Update()
    {
        if (!isShowHandTutorial)
            return;

        if (!isShowingHandTutorial && currentHandTutorialType == HandTutorialType.Drag)
            ShowHandDragTutorial();

        if (UIManager.Instance.IsBlockInput)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            HideHandTutorial();
        }
    }

    private void HideHandTutorial()
    {
        tutorialHand.Toggle(false, false);
        isShowHandTutorial = false;
    }

    private void ShowHandDragTutorial()
    {
        isShowingHandTutorial = true;
        tutorialHand.transform.position = startHandTutorial;
        tutorialHand.transform.DOMove(endHandTutorial, 2f).OnComplete(() => isShowingHandTutorial = false);
    }

    public void ToggleIntroPanel(bool isActive)
    {
        introPanel.SetActive(isActive);
        if (!isActive)
            TutorialController.ON_CLOSE_TUTORIAL_PANEL?.Invoke();
    }

    public void ToggleVideoTutorial(bool isActive)
    {
        videoPanel.SetActive(isActive);
        panel.transform.localScale = isActive ? Vector3.zero : Vector3.one;
        Vector3 desScale = isActive ? Vector3.one : Vector3.zero;
        panel.transform.DOScale(desScale, 0.5f);
        if (!isActive)
            TutorialController.ON_CLOSE_TUTORIAL_PANEL?.Invoke();
    }

    private void PlayVideo()
    {
        ToggleReplayButton(false);
        if (videoPlayer.isPlaying)
            videoPlayer.Stop();
        videoPlayer.Play();
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        ToggleReplayButton(true);
    }

    private void ToggleReplayButton(bool isActive)
    {
        replayButton.gameObject.SetActive(isActive);
    }

    private void ShowHandTapTutorial(Vector3 tapPos)
    {
        tutorialHand.transform.position = tapPos;
        tutorialHand.Toggle(true, false);
        isShowHandTutorial = true;
    }

    private void SetHandTutorialType(HandTutorialType handTutorialType)
    {
        currentHandTutorialType = handTutorialType;
    }

    public void ShowHandTapTutorialAction()
    {
        SetHandTutorialType(HandTutorialType.Tap);
        ShowHandTapTutorial(demoButtons[0].transform.position);
    }

    public void ShowHandTutorialDragAction(Vector3 startPos, Vector3 endPos, Sprite actionSprite)
    {
        SetHandTutorialType(HandTutorialType.Drag);
        tutorialHand.Toggle(true, true);
        isShowHandTutorial = true;
        actionButtonImage.sprite = actionSprite;
        startHandTutorial = startPos;
        endHandTutorial = endPos;
    }

    public void ToggleButtonTutorial(bool isActive)
    {
        buttonPanel.SetActive(isActive);
        if (!isActive)
            TutorialController.ON_CLOSE_TUTORIAL_PANEL?.Invoke();
    }

    public void ToggleDialogText(bool isActive, string content = null)
    {
        textHolder.SetActive(isActive);
        if (isActive)
        {
            PlayNarativeText(content);
        }
    }

    public void PlayNarativeText(string content)
    {
        StartCoroutine(CorPlaynarativeText(content));
    }

    private IEnumerator CorPlaynarativeText(string content)
    {
        string[] stringParts = content.Split("\n");
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < stringParts.Length; i++)
        {
            char[] allChracters = content.ToCharArray();
            int characterCount = content.Length;
            int index = 0;
            while (index < characterCount)
            {
                sb.Append(allChracters[index]);
                index++;
                diaglogText.text = sb.ToString();
                yield return new WaitForSeconds(0.05f);
            }
            diaglogText.text = diaglogText.text + "\n";
        }
    }
}
