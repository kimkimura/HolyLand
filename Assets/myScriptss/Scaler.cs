using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    Vector3 defaultScale;
    // Use this for initialization
    void Start()
    {
        defaultScale = new Vector3 (Mathf.Abs (this.transform.localScale.x), Mathf.Abs (this.transform.localScale.y), Mathf.Abs (this.transform.localScale.z));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newScale = defaultScale;
        Vector3 clampScale = defaultScale * 0.7f;
        float rate = Field.GetScaleInField(this.transform.position);

        newScale.x = Mathf.Lerp(defaultScale.x, clampScale.x, rate);
        newScale.y = Mathf.Lerp(defaultScale.y, clampScale.y, rate);
        if (this.transform.localScale.x < 0)
            newScale.x = -newScale.x;
        this.transform.localScale = newScale; 
    }
}
