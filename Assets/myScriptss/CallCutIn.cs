using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallCutIn : MonoBehaviour
{
    private GameObject cutIn;
    public GameObject hissatsuEffect;
    // Use this for initialization
    void Start ()
    {
        cutIn = GameObject.FindGameObjectWithTag ("CutIn");
    }

    // Update is called once per frame
    void Update ()
    {

    }
    public void CallCutInAnimation ()
    {
        if (cutIn.GetComponent<Animator> ().playbackTime <= 0)
        {
            BattleSystem.SetPose ();
            Pauser.Pause ();
            cutIn.GetComponent<Animator> ().Play ("CutIn");
        }
    }
    public void SetBattleResume ()
    {
        BattleSystem.SetResume (); 
    }
    public void CallEffect ()
    {
        if (hissatsuEffect != null)
            hissatsuEffect.GetComponent<Animator> ().Play ("hissatsu");
    }
}