using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrackingEffect : MonoBehaviour {
    public GameObject target;
    public float damage;
    private const float speed = 4.0f;
    public GameObject damageSkin;
    public GameObject damageEffect;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (target == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            if (Vector2.Distance(target.transform.position, this.transform.position) < 10.0f)
            {
                target.GetComponent<StatusManager>().SetDamage((int)damage);
                Destroy(this.gameObject);
            }
            this.transform.Translate((target.transform.position - this.transform.position) * speed * Time.deltaTime);
        }
	}
}
