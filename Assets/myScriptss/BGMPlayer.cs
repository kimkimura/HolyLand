using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class BGMPlayer : MonoBehaviour {
    public AudioClip BGM;
    private AudioSource audio;
	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource> ();
        audio.clip = BGM;
        audio.loop = true;
        audio.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
