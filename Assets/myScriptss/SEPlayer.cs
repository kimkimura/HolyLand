using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : StateMachineBehaviour {
    public AudioClip soundEffect;
    override public void OnStateEnter (Animator animator, AnimatorStateInfo stateinfo, int layerIndex) {
        AudioSource.PlayClipAtPoint (soundEffect, animator.gameObject.transform.position);
    }
}
