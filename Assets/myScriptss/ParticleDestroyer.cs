using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour {
    public bool quickDestroy = false;
	// Use this for initialization
	void Start () {
        if (quickDestroy)
            Destroy (transform.root.gameObject);
        Destroy(transform.root.gameObject, GetComponent<ParticleSystem>().main.duration);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
