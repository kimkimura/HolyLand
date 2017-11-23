using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {
    //カメラの拡大率（値が小さいほどカメラはズームインし、大きいほどズームアウトする）
    public static float cameraZoomMin = 200;          //シーン内カメラの最大拡大率
    public static float cameraZoomMax = 400;          //シーン内カメラの最小縮小率
    public static float cameraZPos = -100;      //シーン内カメラのZ位置
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public static float GetCameraSizeMin()
    {
        return cameraZoomMin;
    }
    public static float GetCameraSizeMax()
    {
        return cameraZoomMax;
    }
    public static float GetCameraZ()
    {
        return cameraZPos;
    }
}
