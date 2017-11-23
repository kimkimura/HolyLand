using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : testAnimation {
    string[] attackNo = { "Attack1", "Attack2", "Attack3", "Attack4", "Attack5", "Attack6" };
    bool isRunning = false;
	// Use this for initialization
	void Start () {
        rig2d = GetComponent<Rigidbody2D> ();
        speed = 50.0f;
	}
	
	// Update is called once per frame
	void Update () {
        state = anim.GetCurrentAnimatorStateInfo (0);
        if (Random.Range(0,1000)> 990) {
            if(state.normalizedTime > 0.95)
                anim.Play (attackNo[Random.Range (0, attackNo.Length)]);
        }
        else if (Random.Range (0, 1000) > 990) {
            if (Random.Range (0, 100) > 50) {
                rig2d.velocity = Vector2.left * speed;
                Vector3 scale = transform.localScale;
                scale.x = 1;
                transform.localScale = scale;
                StartCoroutine ("Moving");
            }
            else
            {
                rig2d.velocity = Vector2.right * speed;
                Vector3 scale = transform.localScale;
                scale.x = -1;
                transform.localScale = scale;
                StartCoroutine ("Moving");
            }
        }

	}
    IEnumerator Moving () {
        if (isRunning)
            yield break;
        isRunning = true;
        anim.SetBool ("isRunning", true);
        yield return new WaitForSeconds (1.0f);
        stopMove ();
        anim.SetBool ("isRunning", false);
        isRunning = false;
    }
}
