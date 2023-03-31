using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera cameraCaster;
        var go = new GameObject("CameraCaster");
        cameraCaster = go.AddComponent<Camera>();
        cameraCaster.stereoTargetEye = StereoTargetEyeMask.None;
        cameraCaster.fieldOfView = 5f;
        cameraCaster.nearClipPlane = 0.01f;
        cameraCaster.clearFlags = CameraClearFlags.Nothing;
        cameraCaster.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
