using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent (typeof (StatusManager), typeof (Scaler), typeof (Pauser))]
public class AI_Enemy : MonoBehaviour
{
    public int unitType = 0;
    [SerializeField]
    private float margin = 1.0f;
    [SerializeField]
    private float maxRange = 150.0f;
    public string motionName = "Attack1";
    public float[] damage;
    public float attackSpeed;
    private float currentAttackTime;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private bool isLockOn;
    private bool isAttack;
    [SerializeField]
    private string targetTag;
    private Animator myAnim;
    public GameObject skill;
    public GameObject damageSkin;
    public GameObject damageEffect;

    private Master.Status status;
    void Awake ()
    {
        status = GetComponent<StatusManager> ().GetStatus ();
    }
    // Use this for initialization
    void Start ()
    {
        if (this.gameObject.tag == "Enemy")
            targetTag = "Unit";
        else if (this.gameObject.tag == "Unit")
            targetTag = "Enemy";
        target = null;

        //コンポーネント取得
        myAnim = GetComponent<Animator> () ? GetComponent<Animator> () : GetComponentInChildren<Animator> ();
    }

    // Update is called once per frame
    void Update ()
    {

        Vector3 ground = this.transform.Find ("Ground").transform.position;
        Vector3 clampPos = Field.ClampFieldPos (ground);

        ground.z = this.transform.localPosition.z;
        clampPos.z = this.transform.localPosition.z;
        this.transform.localPosition += clampPos - ground;

        if (!BattleSystem.GetIsPose ())
        {
            AnimatorStateInfo state = myAnim.GetCurrentAnimatorStateInfo (0);
                if (state.IsName (motionName))
                    isAttack = true;
            if (!isAttack && !state.IsName ("Damage"))
            {
                if (target != null)
                {
                    if (!CheckDistance ())
                    {
                        target = null;
                    }
                    else
                    {
                        ChangeDirection ();
                        if (Vector2.Distance (target.transform.position, this.transform.position) >= margin)
                        {
                            Vector2 moveDirection = target.transform.position - this.transform.position;
                            this.transform.Translate (moveDirection * Time.deltaTime);


                            myAnim.SetBool ("isRunning", true);
                        }
                        else
                        {
                            myAnim.SetBool ("isRunning", false);
                            AttackTimer ();
                        }
                    }
                }
                else
                {
                    myAnim.SetBool ("isRunning", false);
                    SearchEnemy ();
                }
            }
            else
            {
                if (state.normalizedTime >= 0.9f)
                {
                    isAttack = false;
                }
            }
        }
    }
    void AttackTimer ()
    {
        if (currentAttackTime > 4)
        {
            Attack ();
            currentAttackTime = 0;
        }
        currentAttackTime += (attackSpeed * Random.Range (1.0f, 2.0f)) * Time.deltaTime;
    }
    public void SetKnockBack ()
    {
        myAnim.Play ("Damage");
    }
    public void Attack ()
    {
        //Debug
        myAnim.Play ("Attack1");
    }
    void ChangeDirection ()
    {
        Vector2 scale = this.transform.localScale;
        if (target.transform.position.x - this.transform.position.x > 0)
        {
            scale.x = -Mathf.Abs (scale.x);
        }
        else
        {
            scale.x = Mathf.Abs (scale.x);
        }
        this.transform.localScale = scale;
    }
    bool CheckDistance ()
    {
        if (Vector2.Distance (target.transform.position, this.transform.position) > maxRange)
            return false;
        else
            return true;
    }
    void SearchEnemy ()
    {
        float distance = 0;
        float nearDistance = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag (targetTag))
        {
            distance = Vector2.Distance (obj.transform.position, this.transform.position);
            if (nearDistance == 0 || nearDistance > distance)
            {
                nearDistance = distance;
                if (distance < maxRange)
                    target = obj;
            }
        }
    }
    //void OnTriggerStay2D(Collider2D col) {
    //    Debug.Log(col.gameObject);
    //    if (!isLockOn && col.tag == targetTag)
    //    {
    //        target = col.gameObject;
    //        isLockOn = true;
    //    }
    //}
    //void OnTriggerExit2D(Collider2D col)
    //{
    //    if (col.gameObject == target)
    //    {
    //        target = null;
    //        isLockOn = false;
    //    }
    //}
    //void SearchEnemy()
    //{
    //    if (this.gameObject.tag == "Enemy")
    //    {
    //        GameObject[] targetList = GameObject.FindGameObjectsWithTag("Unit");
    //        target = targetList[Random.Range(0, targetList.Length)];
    //    }
    //    else if (this.gameObject.tag == "Unit")
    //    {
    //        GameObject[] targetList = GameObject.FindGameObjectsWithTag("Enemy");
    //        target = targetList[Random.Range(0, targetList.Length)];
    //    }
    //}
}
