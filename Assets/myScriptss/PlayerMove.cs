using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
[RequireComponent (typeof (StatusManager), typeof (Scaler), typeof (Pauser))]
public class PlayerMove : MonoBehaviour
{
    public int motionType = 0;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 vel = Vector3.zero;
    private Vector2 startPos;
    private Vector2 axis;

    public float speed = 320.0f;
    public float delay = 0.2f;
    public float attackRange = 200f;
    public float damageRate = 200f;
    private bool isYMoving = false;
    private int currentLine = 1;

    private AnimatorStateInfo state;
    private Master.Status status;
    private List<GameObject> skill;

    //各種コンポーネント
    public Animator animator;
    private StatusManager statusManager;

    //マスターデータ
    private Master masterData;

    //画面タッチジェスチャー用コンポーネント
    private TapGesture g_tap;
    private FlickGesture g_flick;
    private TransformGesture g_trans;
    private Gesture g_long;
    void Awake ()
    {
    }
    // Use this for initialization
    void Start ()
    {
        //マスター読み込み・マスターデータ抽出
        masterData = Resources.Load ("MasterData") as Master;

        //コンポーネント読み込み
        animator = GetComponent<Animator> () ? GetComponent<Animator> () : GetComponentInChildren<Animator> ();
        statusManager = GetComponent<StatusManager> ();

        g_tap = GameObject.FindObjectOfType<BattleSystem> ().GetComponent<TapGesture> ();
        g_flick = GameObject.FindObjectOfType<BattleSystem> ().GetComponent<FlickGesture> ();
        g_trans = GameObject.FindObjectOfType<BattleSystem> ().GetComponent<TransformGesture> ();
        g_long = GameObject.FindObjectOfType<BattleSystem> ().GetComponent<Gesture> ();

        //ステータス読み込み
        status = statusManager.GetStatus ();

        //イベントハンドラ登録
        g_tap.Tapped += Attack;
        g_flick.Flicked += LineMove;
        g_trans.TransformStarted += E_StartPos;
        g_trans.Transformed += E_SetAxis;
        g_trans.TransformCompleted += delegate { axis = Vector2.zero; };
    }
    void FixedUpdate ()
    {


    }
    // Update is called once per frame
    void Update ()
    {
         Debug.Log (this.gameObject.name+":"+axis);
        if (!isYMoving)
            BaseMove ();
        state = animator.GetCurrentAnimatorStateInfo (0);
    }
    void Attack (object sender, System.EventArgs e)
    {
        if (BattleSystem.GetIsPose ())
            return;
        Debug.Log ("att");
        state = animator.GetCurrentAnimatorStateInfo (0);
        if (!state.IsName ("knockback") && !state.IsName ("hissatsu"))
        {
            if (state.IsName ("Attack1") && state.normalizedTime > delay)
            {
                animator.Play ("Attack2");

            }
            else if (state.IsName ("Attack2") && state.normalizedTime > delay)
            {
                animator.Play ("Attack3");

            }
            else if (state.IsName ("Attack3") && state.normalizedTime > delay)
            {
                animator.Play ("Attack5");

            }
            else if (state.IsName ("Attack5") && state.normalizedTime > delay)
            {
                animator.Play ("Attack4");

            }
            else if (state.IsName ("Attack4") && state.normalizedTime > delay)
            {
                animator.Play ("Attack6");

            }
            else if (state.IsName ("Idle") || state.IsName ("Run"))
            {
                animator.Play ("Attack1");

            }
        }

    }
    private void E_StartPos (object sender, System.EventArgs e)
    {
            startPos = g_trans.NormalizedScreenPosition;
    }
    private void E_SetAxis (object sender, System.EventArgs e)
    {
        {
            if (g_trans.NormalizedScreenPosition.x - startPos.x > 0)
                axis.x = 1.0f;
            else if (g_trans.NormalizedScreenPosition.x - startPos.x < 0)
                axis.x = -1.0f;
            else
                axis.x = 0;
        }
    }
    private void LineMove (object sender, System.EventArgs e)
    {
        if (BattleSystem.GetIsPose ())
            return;
        Debug.Log ("flick" + g_flick.ScreenFlickVector);
        if (g_flick.ScreenFlickVector.y != 0 && !isYMoving)
        {
            Vector3 ground = GameObject.FindGameObjectWithTag ("PlayerGround").transform.position;
            currentLine = Mathf.Clamp (Field.CheckLine (ground), 1, 3);
            Debug.Log ("currentLine:"+currentLine);
            Debug.Log ("flickAxis:" + g_flick.ScreenFlickVector.y);
            if (g_flick.ScreenFlickVector.y > 0)
                currentLine = Mathf.Clamp (currentLine - 1, 1, 3);
            else if (g_flick.ScreenFlickVector.y < 0)
                currentLine = Mathf.Clamp (currentLine + 1, 1, 3);
            if (currentLine != Field.CheckLine (ground))
            {
                animator.Play ("Step");
                isYMoving = true;
                Vector3 movePos = Field.SetLine (currentLine, this.transform.position);
                ground.z = this.transform.localPosition.z;
                movePos.y += Mathf.Abs (this.transform.position.y - ground.y);
                Hashtable hash = new Hashtable ();
                hash.Add ("x", movePos.x);
                hash.Add ("y", movePos.y);
                hash.Add ("z", movePos.z);
                hash.Add ("time", 0.2f);
                hash.Add ("oncomplete", "DisableYMoving");
                hash.Add ("oncompletetarget", gameObject);
                iTween.MoveTo (gameObject, hash);
            }
        }
    }
    void BaseMove (/*object sender, System.EventArgs e*/)
    {
        if (BattleSystem.GetIsPose ())
            return;
        state = animator.GetCurrentAnimatorStateInfo (0);
        if (axis.x != 0)
        {
            moveDirection.x = speed * axis.x;
            Vector3 scale = this.transform.localScale;
            if (axis.x > 0)
            {
               // Debug.Log (this.gameObject.name+":Left");
                scale.x = -Mathf.Abs (scale.x);
            }
            else
            {
                //Debug.Log (this.gameObject.name+":Right");
                scale.x = Mathf.Abs (scale.x);
            }
            this.transform.localScale = scale;
            animator.SetBool ("isRunning", true);
        }
        else
        {
            moveDirection.x = 0;
            animator.SetBool ("isRunning", false);
        }
        this.transform.Translate (moveDirection * Time.deltaTime);
    }
    void DisableYMoving ()
    {
        Debug.Log ("complete");
        isYMoving = false;
    }
    List<GameObject> SearchEnemy ()
    {
        List<GameObject> attackableTarget = new List<GameObject> ();
        float distance = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag ("Enemy"))
        {
            distance = Vector2.Distance (obj.transform.Find ("Ground").transform.position, this.transform.position);
            if (distance < attackRange)
            {
                attackableTarget.Add (obj);
            }
        }
        return attackableTarget;
    }
    GameObject SearchNearEnemy ()
    {
        GameObject target = null;
        float distance = 0;
        float nearDistance = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag ("Enemy"))
        {
            distance = Vector2.Distance (obj.transform.position, this.transform.position);
            if (nearDistance == 0 || nearDistance > distance)
            {
                nearDistance = distance;
                if (distance < attackRange)
                    target = obj;
            }
        }
        return target;
    }
    public AnimatorStateInfo GetState ()
    {
        return state;
    }
    public void PlayHissatsu ()
    {
        if (!state.IsName ("hissatsu") && BattleSystem.GetGauge () >= 1.0f)
        {
            Debug.Log ("hissatsu");
            //GameObject.FindGameObjectWithTag ("CutIn").GetComponent<CutIn> ().StartCutIn ();
            animator.Play ("hissatsu");
            BattleSystem.SetGauge (-1.0f);
        }
    }
}
