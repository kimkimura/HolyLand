using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Master : ScriptableObject
{
    [System.Serializable]
    public struct Character
    {
        public string CharacterName;
        public int CharacterID;
        public int MotionType;
        public Animator characterAnimator;
        public GameObject Prefab;
        public GameObject[] skills;
        public Sprite CutInIllust;
        public Sprite IconImage;
        public Status StatusData;
    }
    [System.Serializable]
    public struct Enemy
    {
        public string EnemyName;
        public int EnemyID;
        public Animator characterAnimator;
        public GameObject Prefab;
        public GameObject[] skills;
        public Status StatusData;
    }
    [System.Serializable]
    public struct OtherData
    {
        public GameObject DamageSkin;
        public GameObject DamageEffect;
    }
    [System.Serializable]
    public struct Status
    {
        public int HP;
        public int ATK;
        public int DEF;
    }

    public List<Character> characterData;
    public List<Enemy> enemyData;
    public OtherData otherData;
}
