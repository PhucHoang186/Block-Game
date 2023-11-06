using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Image actionIcon;
    public static Action<ActionData> onMouseDownCb;
    private ActionData actionData;

    public void Init(ActionData actionData)
    {
        this.actionData = actionData;
        this.actionIcon.sprite = actionData.actionIcon;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Play " + actionData.actionType);
        onMouseDownCb?.Invoke(actionData);
    }
}
