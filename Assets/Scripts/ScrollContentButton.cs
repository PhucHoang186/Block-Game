using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollContentButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] int direction;
    private bool isHold;
    private Action<int> onUpdateScrollContentCb;

    public void InitAction(Action<int> updateSCrollContent)
    {
        onUpdateScrollContentCb = updateSCrollContent;
    }

    private void Update()
    {
        if (!isHold)
            return;
        onUpdateScrollContentCb?.Invoke(direction);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHold = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHold = false;
    }
}
