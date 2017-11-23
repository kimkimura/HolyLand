using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SStestAnimation : MonoBehaviour {
	public Script_SpriteStudio_Root ss;
	// Use this for initialization
	void Start () {
		ss = GetComponent<Script_SpriteStudio_Root> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
		ss.AnimationPlay (0, 1, 0, 1);
		//ss.FunctionPlayEnd = idleAnimation;
	}
	public void idleAnimation(){
		ss.AnimationPlay (1, 0, 0, 1);
	}
}
