using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStepStore : MonoBehaviour
{
    [field: SerializeField]
    public List<ActionInfo> TutorialSteps { get; private set; }
}
