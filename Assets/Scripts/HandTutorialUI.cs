using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTutorialUI : MonoBehaviour
{
    [SerializeField] GameObject button;

    public void Toggle(bool isActive, bool isShowButton)
    {
        gameObject.SetActive(isActive);
        button.SetActive(isShowButton);
    }
}
