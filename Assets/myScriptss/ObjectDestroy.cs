using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroy : MonoBehaviour {
    public float time = 0;
    public bool CollisionDestroy = false;
    public string targetTag = "";
	// Use this for initialization
	void Start () {
        if (time > 0)
        {
            Destroy(transform.root.gameObject, time);
        }
	}
	
	// Update is called once per frame
	void Update (){
	}
    public void DestroyObjectWithDelay(float delay)
    {
        Destroy(this.gameObject, delay);
    }
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
    public void DestroyRootObject()
    {
        Destroy(transform.root.gameObject);
    }
    public void DestroyTargetObject(GameObject target) {
        Destroy(target.gameObject);
    }
    public void DestroyTargetObjectWithDelay(GameObject target, float delay)
    {
        Destroy(target.gameObject, delay);
    }
    void OnTriggerEnter2D(Collider2D col){
        if (CollisionDestroy)
        {
            if(col.tag == targetTag)
            {
                Destroy(transform.root.gameObject,0.1f);
            }
        }
    }
}
