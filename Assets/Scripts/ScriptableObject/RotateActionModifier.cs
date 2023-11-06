using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RotateActionModifier : ActionModifier
{
    public float rotateAngle;
    public override void ExecuteAction(GameObject player)
    {
        player.TryGetComponent<PlayerCharacter>(out var playerCharacter);
        if (playerCharacter == null)
        {
            Debug.Log("Can't find player. Please Check again");
            return;
        }
        playerCharacter.StartRotate(rotateAngle);

    }
}
