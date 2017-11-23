using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class DamageSender : MonoBehaviour {
    private int damageValue;

    void Start ()
    {
        Rigidbody2D myRig = GetComponent<Rigidbody2D> ();
        myRig.isKinematic = true;
    }
    /// <param name="direction">true= +Scale,false= -Scale</param>
    public void InitDamageInfo (int damage,string tag,bool direction)
    {
        this.damageValue = damage;
        this.tag = tag;
        Vector3 scale = this.transform.root.localScale;
        scale.x = (direction) ? Mathf.Abs (scale.x) : -Mathf.Abs (scale.x);
        this.transform.root.localScale = scale;
    }
    public int GetDamage ()
    {
        return this.damageValue;
    }
}
