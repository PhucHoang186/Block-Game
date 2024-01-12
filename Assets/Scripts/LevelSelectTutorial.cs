using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class LevelSelectTutorial : MonoBehaviour
{
    [SerializeField] GameObject blackBg;
    [SerializeField] GameObject skipButton;
    [SerializeField] GameObject introTextPanel;
    [SerializeField] Transform handTutorial;
    [SerializeField] TMP_Text tutorialText;
    [SerializeField] string tutorialContent;
    private bool isShowTutorial;

    public void ShowFTUE(int childIndex, Transform parent)
    {
        StartCoroutine(CorShowFTUE(childIndex, parent));
    }

    private IEnumerator CorShowFTUE(int childIndex, Transform parent)
    {
        if (GameDataManager.Instance?.GetFTUEState() >= (int)FTUE_State.Intro_Level_Select)
            yield break;
        GameDataManager.Instance?.SaveFTUEStep(FTUE_State.Intro_Level_Select);
        isShowTutorial = true;
        blackBg.SetActive(true);
        blackBg.transform.SetParent(parent);
        blackBg.transform.SetSiblingIndex(childIndex);
        yield return new WaitForSeconds(0.5f);
        introTextPanel.SetActive(true);
        handTutorial.gameObject.SetActive(true);
        handTutorial.transform.position = parent.GetChild(childIndex + 1).transform.position;
        PlayNarativeText(tutorialContent);
    }

    public void SkipTutorialText()
    {
        if (!isShowTutorial)
            return;
        StopAllCoroutines();
        tutorialContent = tutorialContent.Replace("\\n", "\n");
        tutorialText.text = tutorialContent;
        skipButton.SetActive(false);
    }

    public void PlayNarativeText(string content)
    {
        StartCoroutine(CorPlaynarativeText(content));
    }

    private IEnumerator CorPlaynarativeText(string content)
    {
        string[] stringParts = content.Split("\\n");
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < stringParts.Length; i++)
        {
            string contentOfLine = stringParts[i];
            char[] allChracters = contentOfLine.ToCharArray();
            int characterCount = allChracters.Length;
            int index = 0;
            sb.Append("\n");
            while (index < characterCount)
            {
                sb.Append(allChracters[index]);
                index++;
                tutorialText.text = sb.ToString();
                yield return new WaitForSeconds(0.05f);
            }
        }
        skipButton.SetActive(false);
    }
}
