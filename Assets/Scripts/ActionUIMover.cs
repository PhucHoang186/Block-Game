using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUIMover : MonoBehaviour
{
    [SerializeField] Image actionIcon;
    [SerializeField] Transform display;
    [SerializeField] Canvas canvas;
    private Action onReleaseCb;
    public bool IsUsed { get; set; }

    public void InitAction(Action onRelease)
    {
        onReleaseCb = onRelease;
    }

    public void OnStartMoving(Sprite actionIcon)
    {
        ToggleDisplay(true);
        this.actionIcon.sprite = actionIcon;
    }

    public void OnStopMove()
    {
        ToggleDisplay(false);
        onReleaseCb?.Invoke();
    }

    public void ToggleDisplay(bool isActive)
    {
        display.gameObject.SetActive(isActive);
        IsUsed = isActive;
    }

    void Update()
    {
        if (!display.gameObject.activeSelf)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out Vector2 newPosition);
        transform.position = canvas.transform.TransformPoint(newPosition);

        if (Input.GetMouseButtonUp(0))
        {
            OnStopMove();
        }
    }
}
