using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoButton : MonoBehaviour
{
    [SerializeField] ActionType actionType;
    [SerializeField] ActionModifier actionModifier;
    private GameObject playerRender;
    public void Init(GameObject playerRender)
    {
        this.playerRender = playerRender;
    }

    public void PlayAction()
    {
        actionModifier.ExecuteAction(playerRender);
    }
}
