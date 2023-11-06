using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;
    private Animator anim;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        anim = GetComponent<Animator>();
    }

    public void OnCallTransitionIn(bool isCallIn)
    {
        if(isCallIn)
        {
            TransitionIn();
        }
        else
        {
            TransitionOut();
        }
    }

    public void TransitionIn()
    {
        anim.CrossFade("Transition_In", 0f);
    }

    public void TransitionOut()
    {
        anim.CrossFade("Transition_Out", 0f);
    }
}
