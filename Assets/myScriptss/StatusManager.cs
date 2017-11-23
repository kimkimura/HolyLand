using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
public class StatusManager : MonoBehaviour
{

    //※※※※※※※※※※調整中につきInspectorからキャラクターID直打ち※※※※※※※※※※



    private Master masterData;      //マスターデータ

    public GameObject damageSkin;   //ダメージ表記用プレファブ　マスター読み込み
    public GameObject damageEffect; //ダメージエフェクト　　　　マスター読み込み
    public Animator anim;           //キャラクターのアニメータ  マスター読み込み
    public Master.Status status;    //キャラクターのステータス  マスター読み込み
    public int characterID;         //キャラクターID            マスター検索用

    private DamageSpawner sender;

    private string damageTag;       //自身のダメージタグ
    private List<string> targetDamageTag; //被撃対象ダメージタグ
    void Start()
    {
        //マスターデータ読み込み
        masterData = Resources.Load("MasterData") as Master;
        damageSkin = masterData.otherData.DamageSkin;
        damageEffect = masterData.otherData.DamageEffect;

        //各種初期化処理
        targetDamageTag = new List<string>();

        List<GameObject> list = new List<GameObject>();
        switch (this.tag)
        {
            case "Unit":
                {
                    Master.Character data = masterData.characterData.Find(x => x.CharacterID == characterID);
                    InitStatusData(data.StatusData);
                    list.AddRange(data.skills);
                    damageTag = "UnitDamage";
                    targetDamageTag.Add("EnemyDamage");
                    break;
                }
            case "Player":
                {
                    Master.Character data = masterData.characterData.Find(x => x.CharacterID == characterID);
                    InitStatusData(data.StatusData);
                    list.AddRange(data.skills);
                    damageTag = "CharacterDamage";
                    targetDamageTag.Add("EnemyDamage");
                    break;
                }
            case "Enemy":
                {
                    Master.Enemy data = masterData.enemyData.Find(x => x.EnemyID == characterID);
                    InitStatusData(data.StatusData);
                    list.AddRange(data.skills);
                    damageTag = "EnemyDamage";
                    targetDamageTag.Add("UnitDamage");
                    targetDamageTag.Add("CharacterDamage");
                    break;
                }
            default:
                {
                    Master.Status dummyStatus = new Master.Status();
                    dummyStatus.HP = 0;
                    dummyStatus.ATK = 0;
                    dummyStatus.DEF = 0;
                    InitStatusData(dummyStatus);
                    break;
                }
        }
        if (list.Count < 1)
            Debug.Log("list is Empty");
        //コンポーネント取得
        anim = GetComponent<Animator>() ? GetComponent<Animator>() : GetComponentInChildren<Animator>();
        sender = GetComponentInChildren<DamageSpawner>();


        sender.InitSkillData(list);
        sender.InitDamageInfo(status.ATK, damageTag);
    }
    public void Update()
    {
        sender.SetDirection((Mathf.Sign(this.transform.localScale.x) < 0) ? true : false);
    }
    public void InitStatusData(Master.Status stat)
    {
        this.status = stat;
    }
    public void SetDamage(int damageValue)
    {
        //防御力に応じてダメージを軽減
        damageValue = Mathf.Clamp(damageValue - (status.DEF / 2), 0, damageValue - (status.DEF / 2));

        //ダメージ表記とヒットエフェクトの生成
        Vector3 offset = new Vector3(Random.Range(-25.0f, 25.0f), Random.Range(-25.0f, 25.0f), 0);
        GameObject obj = Instantiate(damageSkin, this.transform.position + offset, Quaternion.identity);
        Instantiate(damageEffect, this.transform.position, Quaternion.identity);
        obj.transform.GetComponentInChildren<Text>().text = damageValue.ToString();
        obj.transform.localScale *= Random.Range(0.8f, 1.2f);
        BattleSystem.SetScore(damageValue);
        //ダメージ反映処理
        this.status.HP -= damageValue;
        SetKnockBack();
        //HP0で死亡処理
        if (this.status.HP <= 0)
            Kill();
    }
    //死亡処理
    void Kill()
    {
        Destroy(this.gameObject);
    }

    void SetKnockBack()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("idle"))
            anim.Play("Damage");
    }
    public void SetCharacterID (int id)
    {
        this.characterID = id;
    }
    public Master.Status GetStatus()
    {
        return this.status;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        foreach (string obj in targetDamageTag)
        {
            if (col.tag == obj)
            {
                this.SetDamage(col.GetComponent<DamageSender>().GetDamage());
                if (col.tag == "CharacterDamage")
                    BattleSystem.SetGauge(0.2f);
            }
        }
    }
}
