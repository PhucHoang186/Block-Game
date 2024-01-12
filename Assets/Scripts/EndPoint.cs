using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class EndPoint : MonoBehaviour
{
    [SerializeField] GameObject vfx;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            vfx.SetActive(true);
        }
    }

    public void ShowEndPointIntro()
    {
        anim.CrossFade("Intro", 0f);
    }
}
