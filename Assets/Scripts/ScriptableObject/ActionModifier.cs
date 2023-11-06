using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionModifier : ScriptableObject, IAction
{
    public virtual void ExecuteAction(GameObject player)
    {
    }
}
