using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutIn : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void EndCutIn () {
       //GameObject.FindGameObjectWithTag("Character").GetComponent<Pauser>().OnResume ();
        Pauser.Resume ();
    }
    public void StartCutIn ()
    {
        //Pauser.Pause ();
        BattleSystem.SetPose ();
        GetComponent<Animator> ().Play ("CutIn");
    }
}
