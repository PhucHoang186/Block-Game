using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public enum ActionType
{
    Right,
    Left,
    Up,
    Down,
    RotateLeft,
    RotateRight,
}
[CreateAssetMenu]
public class ActionData : ScriptableObject
{
    public ActionType actionType;
    public Sprite actionIcon;
    public ActionModifier actionModifier;
}


