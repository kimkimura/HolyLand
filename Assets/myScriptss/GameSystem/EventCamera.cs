using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EventCamera : MonoBehaviour {
    [Range(0,1)]
    public float moveRatio = 0;


    private Vector2 bgLT, bgRB, viewPortRectMin, viewPortRectMax;
    private Vector2 moveTarget,moveMin,moveMax;
    private Vector3 velocity = Vector3.zero;
    private static Vector2 moveTo, moveFrom;
    private static MoveMode mode;
    private Camera camera;
    enum MoveMode
    {
        Start,
        Zoom
    }
	// Use this for initialization
	void Start () {
        camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("moveTo:" + moveTo);
        Debug.Log("moveFrom:" + moveFrom);
        moveRatio = Mathf.Clamp(moveRatio, 0, 100);
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, GameSettings.GetCameraSizeMin(), GameSettings.GetCameraSizeMax());
        UpdateStageRect();
        switch (mode)
        {
            case MoveMode.Start:
                {
                    Vector3 movePos = Vector3.zero; 
                    Vector2 pos = Vector2.Lerp(moveFrom, moveTo, moveRatio);
                    movePos.x = Mathf.Clamp(pos.x, moveMin.x, moveMax.x);
                    movePos.y = Mathf.Clamp(pos.y, moveMin.y, moveMax.y);
                    movePos.z = GameSettings.GetCameraZ();
                    Debug.Log(movePos);
                    this.transform.position = Vector3.SmoothDamp(transform.position, movePos,ref velocity,0.5f);
                    break;
                }
            case MoveMode.Zoom:
                {
                    break;
                }
        }
	}
    public void SetTargetPos(Vector2 pos)
    {
        moveTarget = pos;
    }
    public void UpdateStageRect()
    {
        viewPortRectMin = GetComponent<Camera>().ViewportToWorldPoint(Vector2.zero);
        viewPortRectMax = GetComponent<Camera>().ViewportToWorldPoint(Vector2.one);

        SpriteRenderer bg = GameObject.FindGameObjectWithTag("StageBG").GetComponent<SpriteRenderer>();
        bgLT = bg.bounds.min;
        bgRB = bg.bounds.max;

        moveMin.x = bgLT.x + (Mathf.Abs(viewPortRectMax.x - viewPortRectMin.x) / 2.0f);
        moveMin.y = bgLT.y + (Mathf.Abs(viewPortRectMax.y - viewPortRectMin.y) / 2.0f);
        moveMax.x = bgRB.x - (Mathf.Abs(viewPortRectMax.x - viewPortRectMin.x) / 2.0f);
        moveMax.y = bgRB.y - (Mathf.Abs(viewPortRectMax.y - viewPortRectMin.y) / 2.0f);
    }
    public static void SetFrom(Vector2 from) {
        moveFrom = from;
    }
    public static void SetTo(Vector2 to)
    {
        moveTo = to;
    }
    public static void SetStartMode()
    {
        mode = (MoveMode)Enum.Parse(typeof(MoveMode), "Start");
    }
    public static void SetZoomMode()
    {
        mode = (MoveMode)Enum.Parse(typeof(MoveMode), "Zoom");
    }
}
