using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
[RequireComponent (typeof (GameSettings))]
[RequireComponent (typeof (TapGesture), typeof (FlickGesture), typeof (TransformGesture))]
public class BattleSystem : MonoBehaviour
{

    private static bool isPose;         //一時停止の判定

    public string enemyTag;             //敵タグ
    public string unitTag;              //味方タグ

    private Animation cameraAnim;       //メインカメラのAnimationコンポーネント
    private Rect fieldRect;             //フィールドの領域
    private float fieldXMargin = 50.0f; //フィールドの位置調整用
    private float fieldYMargin = 50.0f; //フィールドの位置調整用
    private const float maxGauge = 5;
    public static int score = 0;        //トータルスコア
    public static float gauge = 0;        //必殺ゲージ
    private Text scoreText;
    public Image gauge_main,
                 gauge_sub1,
                 gauge_sub2,
                 gauge_sub3,
                 gauge_sub4,
                 gauge_sub5;
    public Image cutInIllust;

    [Header ("キャラチェンジボタン")]
    public Image BT_Main1;
    public Image BT_Main2;
    public Image BT_Sub1_1;
    public Image BT_Sub1_2;
    public Image BT_Sub2_1;
    public Image BT_Sub2_2;
    public Image BT_Sub3_1;
    public Image BT_Sub3_2;

    //フィールド上のキャラクターオブジェクト格納用
    private GameObject Chara_Main1,
                       Chara_Main2,
                       Chara_Sub1_1,
                       Chara_Sub1_2,
                       Chara_Sub2_1,
                       Chara_Sub2_2,
                       Chara_Sub3_1,
                       Chara_Sub3_2;

    //マスターデータ
    private Master masterData;

    private Sprite Chara_Main1_Illust;
    private Sprite Chara_Main2_Illust;

    //Debug
    [Header ("登場させるキャラID(0は出現なし)")]
    public int Chara_Main1_ID = 2;
    public int Chara_Main2_ID = 3;
    public int Chara_Sub1_1_ID = 1;
    public int Chara_Sub1_2_ID = 0;
    public int Chara_Sub2_1_ID = 4;
    public int Chara_Sub2_2_ID = 0;
    public int Chara_Sub3_1_ID = 0;
    public int Chara_Sub3_2_ID = 0;

    void Awake ()
    {
        masterData = Resources.Load ("MasterData") as Master;
        cameraAnim = Camera.main.GetComponent<Animation> ();
        scoreText = GameObject.FindGameObjectWithTag ("Score").GetComponent<Text> ();
        score = 0;
        gauge = 0;

        //マスターから読み込み
        //プレファブ
        Chara_Main1 = masterData.characterData.Find (x => x.CharacterID == Chara_Main1_ID).Prefab;
        Chara_Main2 = masterData.characterData.Find (x => x.CharacterID == Chara_Main2_ID).Prefab;
        Chara_Sub1_1 = masterData.characterData.Find (x => x.CharacterID == Chara_Sub1_1_ID).Prefab;
        Chara_Sub1_2 = masterData.characterData.Find (x => x.CharacterID == Chara_Sub1_2_ID).Prefab;
        Chara_Sub2_1 = masterData.characterData.Find (x => x.CharacterID == Chara_Sub2_1_ID).Prefab;
        Chara_Sub2_2 = masterData.characterData.Find (x => x.CharacterID == Chara_Sub2_2_ID).Prefab;
        Chara_Sub3_1 = masterData.characterData.Find (x => x.CharacterID == Chara_Sub3_1_ID).Prefab;
        Chara_Sub3_2 = masterData.characterData.Find (x => x.CharacterID == Chara_Sub3_2_ID).Prefab;

        //アイコン
        BT_Main1.sprite = masterData.characterData.Find (x => x.CharacterID == Chara_Main1_ID).IconImage;
        BT_Main2.sprite = masterData.characterData.Find (x => x.CharacterID == Chara_Main2_ID).IconImage;
        BT_Sub1_1.sprite = masterData.characterData.Find (x => x.CharacterID == Chara_Sub1_1_ID).IconImage;
        BT_Sub1_2.sprite = masterData.characterData.Find (x => x.CharacterID == Chara_Sub1_2_ID).IconImage;
        BT_Sub2_1.sprite = masterData.characterData.Find (x => x.CharacterID == Chara_Sub2_1_ID).IconImage;
        BT_Sub2_2.sprite = masterData.characterData.Find (x => x.CharacterID == Chara_Sub2_2_ID).IconImage;
        BT_Sub3_1.sprite = masterData.characterData.Find (x => x.CharacterID == Chara_Sub3_1_ID).IconImage;
        BT_Sub3_2.sprite = masterData.characterData.Find (x => x.CharacterID == Chara_Sub3_2_ID).IconImage;

        //カットイン用イラスト
        Chara_Main1_Illust = masterData.characterData.Find (x => x.CharacterID == Chara_Main1_ID).CutInIllust;
        Chara_Main2_Illust = masterData.characterData.Find (x => x.CharacterID == Chara_Main2_ID).CutInIllust;

        //配置
        UnitFormation formation = GetComponent<UnitFormation> ();
        UnitFormation.unit unitForm = formation.GetUnitFormation ();

        Vector3 mainPos = unitForm.unitPos5;
        Vector3 subPos1 = unitForm.unitPos7;
        Vector3 subPos2 = unitForm.unitPos9;
        Vector3 subPos3 = unitForm.unitPos8;

        //生成
        if (Chara_Main1 != null)
        {
            Chara_Main1 = Instantiate (Chara_Main1, mainPos, Quaternion.identity);
            Chara_Main1.transform.localScale = SetScale (Chara_Main1.transform.localScale, true);
            Chara_Main1.AddComponent<PlayerMove> ();
            Chara_Main1.tag = "Player";
            Chara_Main1.GetComponent<StatusManager> ().SetCharacterID (Chara_Main1_ID);
            Chara_Main1.transform.Find ("Ground").tag = "PlayerGround";
        }
        else
        {
            BT_Main1.color = Color.clear;
        }
        if (Chara_Main2 != null)
        {
            Chara_Main2 = Instantiate (Chara_Main2, mainPos, Quaternion.identity);
            Chara_Main2.transform.localScale = SetScale (Chara_Main2.transform.localScale, true);
            Chara_Main2.AddComponent<PlayerMove> ();
            Chara_Main2.tag = "Player";
            Chara_Main2.GetComponent<StatusManager> ().SetCharacterID (Chara_Main2_ID);
            Chara_Main2.transform.Find ("Ground").tag = "PlayerGround";

        }
        else
        {
            BT_Main2.color = Color.clear;
        }
        if (Chara_Sub1_1 != null)
        {
            Chara_Sub1_1 = Instantiate (Chara_Sub1_1, subPos1, Quaternion.identity);
            Chara_Sub1_1.transform.localScale = SetScale (Chara_Sub1_1.transform.localScale, true);
            Chara_Sub1_1.AddComponent<AI_Enemy> ();
            Chara_Sub1_1.tag = unitTag;
            Chara_Sub1_1.GetComponent<StatusManager> ().SetCharacterID (Chara_Sub1_1_ID);
        }
        else
        {
            BT_Sub1_1.color = Color.clear;
        }
        if (Chara_Sub1_2 != null)
        {
            Chara_Sub1_2 = Instantiate (Chara_Sub1_2, subPos1, Quaternion.identity);
            Chara_Sub1_2.transform.localScale = SetScale (Chara_Sub1_2.transform.localScale, true);
            Chara_Sub1_2.AddComponent<AI_Enemy> ();
            Chara_Sub1_2.tag = unitTag;
            Chara_Sub1_2.GetComponent<StatusManager> ().SetCharacterID (Chara_Sub1_2_ID);
        }
        else
        {
            BT_Sub1_2.color = Color.clear;
        }
        if (Chara_Sub2_1 != null)
        {
            Chara_Sub2_1 = Instantiate (Chara_Sub2_1, subPos2, Quaternion.identity);
            Chara_Sub2_1.transform.localScale = SetScale (Chara_Sub2_1.transform.localScale, true);
            Chara_Sub2_1.AddComponent<AI_Enemy> ();
            Chara_Sub2_1.tag = unitTag;
            Chara_Sub2_1.GetComponent<StatusManager> ().SetCharacterID (Chara_Sub2_1_ID);
        }
        else
        {
            BT_Sub2_1.color = Color.clear;
        }
        if (Chara_Sub2_2 != null)
        {
            Chara_Sub2_2 = Instantiate (Chara_Sub2_2, subPos2, Quaternion.identity);
            Chara_Sub2_2.transform.localScale = SetScale (Chara_Sub2_2.transform.localScale, true);
            Chara_Sub2_2.AddComponent<AI_Enemy> ();
            Chara_Sub2_2.tag = unitTag;
            Chara_Sub2_2.GetComponent<StatusManager> ().SetCharacterID (Chara_Sub2_2_ID);
        }
        else
        {
            BT_Sub2_2.color = Color.clear;
        }
        if (Chara_Sub3_1 != null)
        {
            Chara_Sub3_1 = Instantiate (Chara_Sub3_1, subPos3, Quaternion.identity);
            Chara_Sub3_1.transform.localScale = SetScale (Chara_Sub3_1.transform.localScale, true);
            Chara_Sub3_1.AddComponent<AI_Enemy> ();
            Chara_Sub3_1.tag = unitTag;
            Chara_Sub3_1.GetComponent<StatusManager> ().SetCharacterID (Chara_Sub3_1_ID);
        }
        else
        {
            BT_Sub3_1.color = Color.clear;
        }
        if (Chara_Sub3_2 != null)
        {
            Chara_Sub3_2 = Instantiate (Chara_Sub3_2, subPos3, Quaternion.identity);
            Chara_Sub3_2.transform.localScale = SetScale (Chara_Sub3_2.transform.localScale, true);
            Chara_Sub3_2.AddComponent<AI_Enemy> ();
            Chara_Sub3_2.tag = unitTag;
            Chara_Sub3_2.GetComponent<StatusManager> ().SetCharacterID (Chara_Sub3_2_ID);
        }
        else
        {
            BT_Sub3_2.color = Color.clear;
        }

        //サブキャラクターの非表示
        if (Chara_Main2 != null)
            Chara_Main2.SetActive (false);
        if (Chara_Sub1_2 != null)
            Chara_Sub1_2.SetActive (false);
        if (Chara_Sub2_2 != null)
            Chara_Sub2_2.SetActive (false);
        if (Chara_Sub3_2 != null)
            Chara_Sub3_2.SetActive (false);


    }
    void Start ()
    {

        BattleStartEvent ();
    }

    void Update ()
    {
        SetGaugeUI ();
        SetScoreText ();
        if (!GetIsPose ())
        {
            //UpdateUnitPos ();
            if (CheckClear ())
            {
                BattleEndEvent ();
            }
        }
    }
    //ステージ初期化処理
    private void InitStage ()
    {
        fieldRect = Field.GetField ();
    }
    /*
    void LoadMaster ()
    {
        if (this.tag == "Unit")//味方データから読み込み
        {
            for (int i = 0; i < masterData.characterData.Length; i++)
            {
                if (masterData.characterData[i].CharacterID == characterID)
                {

                }
            }
        }
        else　　　　　　　　　 //敵データから読み込み
        {
            for (int i = 0; i < masterData.enemyData.Length; i++)
            {
                if (masterData.enemyData[i].EnemyID == characterID)
                {

                }
            }
        }
    }
     */
    //バトル開始前の演出処理
    private void BattleStartEvent ()
    {
        SetPose ();
        cameraAnim.Play ("StartCamera");
    }
    //バトル終了時の演出処理
    private void BattleEndEvent ()
    {
        SetPose ();
        cameraAnim.Play ("ClearCamera");
    }
    //フィールド上の敵殲滅チェック
    private bool CheckClear ()
    {
        GameObject[] fieldEnemy = GameObject.FindGameObjectsWithTag (enemyTag);
        if (fieldEnemy.Length < 1)
            return true;
        else return false;
    }
    //指定されたタグの陣営の平均位置を返す
    public static Vector2 GetPartyPos (string tag)
    {
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag (tag);
        Vector3 sum = Vector3.zero;
        for (int i = 0; i < targetObjects.Length; i++)
        {
            sum += targetObjects[i].transform.position;
        }
        Vector2 pos = sum / targetObjects.Length;
        return pos;
    }
    //ポーズ中かどうかのチェック
    public static bool GetIsPose ()
    {
        return isPose;
    }
    //ポーズ状態にする
    public static void SetPose ()
    {
        isPose = true;
    }
    //ポーズ状態の解除
    public static void SetResume ()
    {
        isPose = false;
    }
    //スコアの反映
    void SetScoreText ()
    {
        scoreText.text = score.ToString ();
    }
    //ゲージ加算
    public static void SetGauge (float value)
    {
        gauge = Mathf.Clamp (gauge + value, 0, maxGauge);
    }
    public static float GetGauge ()
    {
        return gauge;
    }
    //ゲージ反映
    void SetGaugeUI ()
    {
        gauge_main.fillAmount = Mathf.Clamp (Mathf.Repeat (gauge, 1), 0, 1);
        gauge_sub1.enabled = (gauge >= 1);
        gauge_sub2.enabled = (gauge >= 2);
        gauge_sub3.enabled = (gauge >= 3);
        gauge_sub4.enabled = (gauge >= 4);
        gauge_sub5.enabled = (gauge >= 5);
    }
    //スコア加算
    public static void SetScore (int value)
    {
        score += value;
    }
    //必殺アニメーション呼び出し
    public void CallHissatsu ()
    {
        if (GetIsPose ())
            return;
        PlayerMove playerMove = GameObject.FindObjectOfType<PlayerMove> ().GetComponent<PlayerMove> ();
        playerMove.PlayHissatsu ();

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj">対象のscale</param>
    /// <param name="direction">true=right,false=left</param>
    private Vector3 SetScale (Vector3 target, bool direction)
    {
        Vector3 scale = target;
        scale.x = (direction) ? -Mathf.Abs (scale.x) : Mathf.Abs (scale.x);
        return scale;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="buttonNumber">0=Main,1=Sub1,2=Sub2,3=Sub3</param>
    public void ChangeCharacter (int buttonNumber)
    {
        if (GetIsPose ())
            return;
        switch (buttonNumber)
        {
            case 0:
                {

                    if (Chara_Main2 == null || Chara_Main1 == null)
                        break;
                    if (!Chara_Main1.activeSelf)
                    {
                        Chara_Main1.SetActive (true);
                        Chara_Main2.SetActive (false);
                        BT_Main1.CrossFadeColor (Color.white, 0.5f, true, false);
                        BT_Main2.CrossFadeColor (Color.gray, 0.5f, true, false);
                        Chara_Main1.transform.position = Chara_Main2.transform.position;
                        Vector3 scale = Chara_Main1.transform.localScale;
                        scale.x = Mathf.Abs (scale.x) * Mathf.Clamp (Chara_Main2.transform.localScale.x, -1.0f, 1.0f);
                        Chara_Main1.transform.localScale = scale;
                        cutInIllust.sprite = Chara_Main1_Illust;
                    }
                    else
                    {
                        Chara_Main1.SetActive (false);
                        Chara_Main2.SetActive (true);
                        BT_Main1.CrossFadeColor (Color.gray, 0.5f, true, false);
                        BT_Main2.CrossFadeColor (Color.white, 0.5f, true, false);
                        Chara_Main2.transform.position = Chara_Main1.transform.position;
                        Vector3 scale = Chara_Main2.transform.localScale;
                        scale.x = Mathf.Abs (scale.x) * Mathf.Clamp (Chara_Main1.transform.localScale.x, -1.0f, 1.0f);
                        Chara_Main2.transform.localScale = scale;
                        cutInIllust.sprite = Chara_Main2_Illust;
                    }
                    break;
                }
            case 1:
                {
                    if (Chara_Sub1_2 == null || Chara_Sub1_1 == null)
                        break;
                    if (!Chara_Sub1_1.activeSelf)
                    {
                        Chara_Sub1_1.SetActive (true);
                        Chara_Sub1_2.SetActive (false);
                        BT_Sub1_1.CrossFadeColor (Color.white, 0.5f, true, false);
                        BT_Sub1_2.CrossFadeColor (Color.gray, 0.5f, true, false);
                        Chara_Sub1_1.transform.position = Chara_Sub1_2.transform.position;
                        Vector3 scale = Chara_Sub1_1.transform.localScale;
                        scale.x = Mathf.Abs (scale.x) * Mathf.Clamp (Chara_Sub1_2.transform.localScale.x, -1.0f, 1.0f);
                        Chara_Sub1_1.transform.localScale = scale;
                    }
                    else
                    {
                        Chara_Sub1_1.SetActive (false);
                        Chara_Sub1_2.SetActive (true);
                        BT_Sub1_1.CrossFadeColor (Color.gray, 0.5f, true, false);
                        BT_Sub1_2.CrossFadeColor (Color.white, 0.5f, true, false);
                        Chara_Sub1_2.transform.position = Chara_Sub1_1.transform.position;
                        Vector3 scale = Chara_Sub1_2.transform.localScale;
                        scale.x = Mathf.Abs (scale.x) * Mathf.Clamp (Chara_Sub1_1.transform.localScale.x, -1.0f, 1.0f);
                        Chara_Sub1_2.transform.localScale = scale;
                    }
                    break;
                }
            case 2:
                {
                    if (Chara_Sub2_1 == null || Chara_Sub2_2 == null)
                        break;
                    if (!Chara_Sub2_1.activeSelf)
                    {
                        Chara_Sub2_1.SetActive (true);
                        Chara_Sub2_2.SetActive (false);
                        BT_Sub2_1.CrossFadeColor (Color.white, 0.5f, true, false);
                        BT_Sub2_2.CrossFadeColor (Color.gray, 0.5f, true, false);
                        Chara_Sub2_1.transform.position = Chara_Sub2_2.transform.position;
                        Vector3 scale = Chara_Sub2_1.transform.localScale;
                        scale.x = Mathf.Abs (scale.x) * Mathf.Clamp (Chara_Sub2_2.transform.localScale.x, -1.0f, 1.0f);
                        Chara_Sub2_1.transform.localScale = scale;
                    }
                    else
                    {
                        Chara_Sub2_1.SetActive (false);
                        Chara_Sub2_2.SetActive (true);
                        BT_Sub2_1.CrossFadeColor (Color.gray, 0.5f, true, false);
                        BT_Sub2_2.CrossFadeColor (Color.white, 0.5f, true, false);
                        Chara_Sub2_2.transform.position = Chara_Sub2_1.transform.position;
                        Vector3 scale = Chara_Sub2_2.transform.localScale;
                        scale.x = Mathf.Abs (scale.x) * Mathf.Clamp (Chara_Sub2_1.transform.localScale.x, -1.0f, 1.0f);
                        Chara_Sub2_2.transform.localScale = scale;
                    }
                    break;
                }
            case 3:
                {
                    if (Chara_Sub3_1 == null || Chara_Sub3_2 == null)
                        break;
                    if (!Chara_Sub3_1.activeSelf)
                    {
                        Chara_Sub3_1.SetActive (true);
                        Chara_Sub3_2.SetActive (false);
                        BT_Sub3_1.CrossFadeColor (Color.white, 0.5f, true, false);
                        BT_Sub3_2.CrossFadeColor (Color.gray, 0.5f, true, false);
                        Chara_Sub3_1.transform.position = Chara_Sub3_2.transform.position;
                        Vector3 scale = Chara_Sub3_1.transform.localScale;
                        scale.x = Mathf.Abs (scale.x) * Mathf.Clamp (Chara_Sub3_2.transform.localScale.x, -1.0f, 1.0f);
                        Chara_Sub3_1.transform.localScale = scale;
                    }
                    else
                    {
                        Chara_Sub3_1.SetActive (false);
                        Chara_Sub3_2.SetActive (true);
                        BT_Sub3_1.CrossFadeColor (Color.gray, 0.5f, true, false);
                        BT_Sub3_2.CrossFadeColor (Color.white, 0.5f, true, false);
                        Chara_Sub3_2.transform.position = Chara_Sub3_1.transform.position;
                        Vector3 scale = Chara_Sub3_2.transform.localScale;
                        scale.x = Mathf.Abs (scale.x) * Mathf.Clamp (Chara_Sub3_1.transform.localScale.x, -1.0f, 1.0f);
                        Chara_Sub3_2.transform.localScale = scale;
                    }
                    break;
                }
            default: break;
        }
    }
}
