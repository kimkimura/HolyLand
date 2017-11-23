using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoomCamera : MonoBehaviour {
    private Camera myCamera;
    private const float zoomInSize = 80.0f;
    private const float zoomOutSize = 180.0f;
    private bool zoomFlag;
	// Use this for initialization
	void Start () {
        zoomFlag = false;
        myCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (zoomFlag)
            myCamera.orthographicSize = zoomInSize;
        else
            myCamera.orthographicSize = zoomOutSize;

        if (Input.GetKeyDown (KeyCode.U))
            zoomFlag = !zoomFlag;
	}
}
