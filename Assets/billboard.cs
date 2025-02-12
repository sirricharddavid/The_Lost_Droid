using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billboard : MonoBehaviour
{
    public GameObject cam;
    public Transform camTransform;

    Quaternion originalRotation;

    void Start()
    {
        cam = GameObject.Find("Main Camera");
        camTransform = cam.transform;
        originalRotation = transform.rotation;
    }

    void LateUpdate()
    {
        //transform.rotation = camTransform.rotation * originalRotation;
        transform.LookAt(transform.position + camTransform.rotation * Vector3.forward, camTransform.rotation * Vector3.up);
    }
}