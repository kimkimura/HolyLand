using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour {
    public ParticleSystem[] particle;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void playParticle(int index)
    {
        if (particle[index] != null)
            particle[index].Play();
    }
}
