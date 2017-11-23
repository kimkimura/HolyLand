using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlayer : StateMachineBehaviour {
    public AudioClip soundEffect;
    override public void OnStateEnter(Animator animator,AnimatorStateInfo stateInfo,int layerIndex){
        AudioSource.PlayClipAtPoint (soundEffect, animator.gameObject.transform.position);
    }
}
