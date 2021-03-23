using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform CamTransform;
    void Start()
    {
        CamTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + CamTransform.rotation * Vector3.forward, CamTransform.rotation * Vector3.up);
    }
}
