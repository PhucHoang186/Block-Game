using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotatePoint : MonoBehaviour
{
    [SerializeField] float rotatetSpeed;

    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y + rotatetSpeed, 0f)), Time.deltaTime);
    }
}
