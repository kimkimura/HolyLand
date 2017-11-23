using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectSpawn : MonoBehaviour {
	public GameObject[] spawnEffectObject;
	enum Effect{
		Slash = 0,
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void spawnEffect(GameObject targetObject,string effectName){
		int effectNumber = (int)Enum.Parse (typeof(Effect), effectName);
		Instantiate (spawnEffectObject [effectNumber],targetObject.transform.position,Quaternion.identity);
	}
    public void spawnSingleEffect(int index)
    {
        Instantiate(spawnEffectObject[index], this.transform.position, Quaternion.identity);
    }

}
