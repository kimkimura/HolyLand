using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private GameObject player;
    private GameObject defaultPos;
    private GameObject currentTarget;
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private float zPos;
    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        //   defaultPos = GameObject.FindGameObjectWithTag ("DefaultPos"); 
        // SetDefaultPos ();
        SetPlayerPos ();

    }

    // Update is called once per frame
    void Update ()
    {
        //this.transform.position = Vector2.SmoothDamp (this.transform.position, currentTarget.transform.position, ref velocity, 0.5f, 5.0f, Time.deltaTime);
        Vector3 targetPos = currentTarget.transform.position;
        targetPos.z = zPos;
        this.transform.position = Vector3.SmoothDamp (this.transform.position, targetPos, ref velocity, 0.5f/*Time.deltaTime*/);
    }
    public void SetPlayerPos ()
    {
        currentTarget = player;
    }
    public void SetDefaultPos ()
    {
        currentTarget = defaultPos;
    }
}
