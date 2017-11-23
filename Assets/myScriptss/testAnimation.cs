using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAnimation : MonoBehaviour
{
    public Animator anim;
    // public  CharacterVoice;
    //public Animator cutIn;
    //public Animator hissatsuEffect;
    protected AnimatorStateInfo state;
    protected Rigidbody2D rig2d;
    public float attDelay1 = 0;
    public float attDelay2 = 0;
    public float attDelay3 = 0;
    public float attDelay4 = 0;
    public float attDelay5 = 0;
    public float attDelay6 = 0;
    protected const float progress = 0.3f;
    protected float speed = 150.0f;
    protected const float maxZpos = 100.0f;
    // Use this for initialization
    void Start ()
    {
       // anim = GameObject.FindGameObjectWithTag ("Character").GetComponent<Animator> ();
        rig2d = GetComponent<Rigidbody2D> ();
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 pos = this.transform.position;
        pos.z = Mathf.Clamp (pos.z, 0, maxZpos);
        this.transform.position = pos;

        state = anim.GetCurrentAnimatorStateInfo (0);

        if (state.normalizedTime > progress && !state.IsName ("hissatsu"))
        {
            if (Input.GetKeyDown (KeyCode.B))
                anim.Play ("knockback");
            if (!state.IsName ("knockback") && state.normalizedTime > progress)
            {
                if (Input.GetKeyDown (KeyCode.V))
                    anim.Play ("hissatsu");
                //if (Input.GetKeyDown (KeyCode.T))
                    //cutIn.Play ("CutIn");
                    //if (Input.GetKeyDown (KeyCode.U))
                        //hissatsuEffect.Play ("HissatsuSlash");
                        if (Input.GetKeyDown (KeyCode.Space))
                        {
                            if (state.IsName ("Attack1") && state.normalizedTime > progress + attDelay1)
                                anim.Play ("Attack2", 0, 0.0f);
                            else if (state.IsName ("Attack2") && state.normalizedTime > progress + attDelay2)
                                anim.Play ("Attack3", 0, 0.0f);
                            else if (state.IsName ("Attack3") && state.normalizedTime > progress + attDelay3)
                                anim.Play ("Attack5", 0, 0.0f);
                            else if (state.IsName ("Attack5") && state.normalizedTime > progress + attDelay4)
                                anim.Play ("Attack4", 0, 0.0f);
                            else if (state.IsName ("Attack4") && state.normalizedTime > progress + attDelay5)
                                anim.Play ("Attack6", 0, 0.0f);
                            else if (state.IsName ("Attack4") && state.normalizedTime > progress + attDelay6)
                                anim.Play ("Attack6", 0, 0.0f);
                            else
                                anim.Play ("Attack1", 0, 0.0f);
                        }
            }
            if (state.IsName ("Idle") || state.IsName ("Run"))
            {
                if (Input.GetKey ("left") || Input.GetKey ("right") || Input.GetKey ("up") || Input.GetKey ("down"))
                {
                    if (Input.GetKey ("left"))
                    {
                        rig2d.velocity = Vector2.left * speed;
                        Vector3 scale = transform.localScale;
                        scale.x = 1;
                        transform.localScale = scale;
                    }
                    if (Input.GetKey ("right"))
                    {
                        rig2d.velocity = Vector2.right * speed;
                        Vector3 scale = transform.localScale;
                        scale.x = -1;
                        transform.localScale = scale;
                    }
                    if (Input.GetKey ("up"))
                    {
                        this.transform.Translate (0, 0, 1);
                    }
                    if (Input.GetKey ("down"))
                    {
                        this.transform.Translate (0, 0, -1);
                    }
                    anim.SetBool ("isRunning", true);
                }
                else
                {
                    anim.SetBool ("isRunning", false);
                    stopMove ();
                }
            }
        }
        else
        {
            stopMove ();
        }
    }
    protected void stopMove ()
    {
        Vector2 vel = rig2d.velocity;
        vel.x = 0;
        rig2d.velocity = vel;
    }
}
