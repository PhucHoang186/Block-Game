using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MainMenuCameraController : MonoBehaviour
{
    [SerializeField] List<CinemachineVirtualCamera> vCams;
    [SerializeField] float randomTime = 3f;
    private float currentRandowmTime;

    void Start()
    {
        currentRandowmTime = randomTime;
    }


    void Update()
    {
        if(currentRandowmTime <= 0)
        {
            currentRandowmTime = randomTime;
            ChangeCemraAngle();
        }
        else
        {
            currentRandowmTime -= Time.deltaTime;
        }
    }

    void OnDestroy()
    {
        CancelInvoke();
    }

    private void ChangeCemraAngle()
    {
        var cam = GetRandowmCamera();
        SetCameraPriority(cam);
    }

    private void SetCameraPriority(CinemachineVirtualCamera cam)
    {
        for (int i = 0; i < vCams.Count; i++)
        {
            if (vCams[i] == cam)
            {
                vCams[i].Priority = 99;
            }
            else
            {
                vCams[i].Priority = 10;
            }
        }
    }

    private CinemachineVirtualCamera GetRandowmCamera()
    {
        int random = Random.Range(0, vCams.Count);
        return vCams[random];
    }
}
