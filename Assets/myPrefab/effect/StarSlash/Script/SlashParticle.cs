using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashParticle : MonoBehaviour {
    public ParticleSystem hit,
                          particle1,
                          particle2,
                          particle3,
                          particle4,
                          particle5;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
    public void play1 ()
    {
        particle1.Play ();
    }
    public void play2 ()
    {
        particle2.Play ();
    }
    public void play3 ()
    {
        particle3.Play ();
    }
    public void play4 () 
    {
        particle4.Play ();
    }
    public void play5 () 
    {
        particle5.Play ();
    }
    public void playHit () {
        hit.Play ();
    }
}