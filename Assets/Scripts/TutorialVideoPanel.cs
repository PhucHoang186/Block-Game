using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
using System.Text;
using TMPro;

public class TutorialVideoPanel : MonoBehaviour
{
    [SerializeField] GameObject display;
    [SerializeField] Transform panel;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] Button replayButton;

    [SerializeField] GameObject textHolder;
    [SerializeField] TMP_Text diaglogText;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        replayButton.onClick.AddListener(PlayVideo);
    }

    void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
        replayButton.onClick.RemoveListener(PlayVideo);
    }

    public void ToggleVideoTutorial(bool isActive)
    {
        display.SetActive(isActive);
        panel.transform.localScale = isActive ? Vector3.zero : Vector3.one;
        Vector3 desScale = isActive ? Vector3.one : Vector3.zero;
        panel.transform.DOScale(desScale, 0.5f);
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

    public void ToggleDialogText(bool isActive, string content = null)
    {
        textHolder.SetActive(isActive);
        if(isActive)
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
        StringBuilder sb = new StringBuilder();
        char[] allChracters = content.ToCharArray();
        int characterCount = content.Length;
        int i = 0;
        while (i < characterCount)
        {
            sb.Append(allChracters[i]);
            i++;
            diaglogText.text = sb.ToString();
            yield return new WaitForSeconds(0.05f);
        }

    }
}
