using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [SerializeField] GameObject vfx;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            vfx.SetActive(true);
            var player = collider.GetComponent<PlayerCharacter>();
            if (player != null)
                player.Celebrate();
        }
    }
}
