using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpawner : MonoBehaviour
{
    List<GameObject> skillList;
    private string myTag;
    private int myBaseDamage;
    private bool direction;
    public void InitSkillData (List<GameObject> skills)
    {
        this.skillList = skills;
        Debug.Log (skills[0]);
    }
    public void SetSkill (int skillNo)
    {
        GameObject spawnObj =
        Instantiate (skillList[skillNo], transform.position, Quaternion.identity);

        DamageSender sender = (spawnObj.GetComponent<DamageSender> ()) ? spawnObj.GetComponent<DamageSender> () : (spawnObj.GetComponentInChildren<DamageSender> ()) ? spawnObj.GetComponentInChildren<DamageSender> () : null;
        if (sender != null)
            sender.InitDamageInfo (myBaseDamage, myTag, this.direction);
        Vector3 scale = spawnObj.transform.localScale;
        scale.x = Mathf.Abs (spawnObj.transform.localScale.x) * -Mathf.Clamp (transform.root.localScale.x, -1.0f, 1.0f);
        spawnObj.transform.localScale = scale;
    }
    public void InitDamageInfo (int damage, string tag)
    {
        this.myBaseDamage = damage;
        this.myTag = tag;
        this.direction = true;
    }
    /// <param name="dir">true= +Scale,false= -Scale</param>
    public void SetDirection (bool dir)
    {
        this.direction = dir;
    }
}
