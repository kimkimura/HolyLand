using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class UnitFormation : MonoBehaviour
{
    public enemy enemyFormation;
    public unit unitFormation;
    [System.Serializable]
    public struct enemy
    {
        public Vector3 enemyPos1;
        public Vector3 enemyPos2;
        public Vector3 enemyPos3;
        public Vector3 enemyPos4;
        public Vector3 enemyPos5;
        public Vector3 enemyPos6;
        public Vector3 enemyPos7;
        public Vector3 enemyPos8;
        public Vector3 enemyPos9;
        public Vector3 enemyPos10;
        public Vector3 enemyPos11;
        public Vector3 enemyPos12;
    }
    [System.Serializable]
    public struct unit
    {
        public Vector3 unitPos1;
        public Vector3 unitPos2;
        public Vector3 unitPos3;
        public Vector3 unitPos4;
        public Vector3 unitPos5;
        public Vector3 unitPos6;
        public Vector3 unitPos7;
        public Vector3 unitPos8;
        public Vector3 unitPos9;
        public Vector3 unitPos10;
        public Vector3 unitPos11;
        public Vector3 unitPos12;
    }
    public unit GetUnitFormation ()
    {
        return this.unitFormation;
    }
    public enemy GetEnemyFormation ()
    {
        return this.enemyFormation;
    }
#if UNITY_EDITOR
    void OnDrawGizmosSelected ()
    {
        Gizmos.DrawIcon (enemyFormation.enemyPos1, "Number_01", false);
        Gizmos.DrawIcon (enemyFormation.enemyPos2, "Number_02", false);
        Gizmos.DrawIcon (enemyFormation.enemyPos3, "Number_03", false);
        Gizmos.DrawIcon (enemyFormation.enemyPos4, "Number_04", false);
        Gizmos.DrawIcon (enemyFormation.enemyPos5, "Number_05", false);
        Gizmos.DrawIcon (enemyFormation.enemyPos6, "Number_06", false);
        Gizmos.DrawIcon (enemyFormation.enemyPos7, "Number_07", false);
        Gizmos.DrawIcon (enemyFormation.enemyPos8, "Number_08", false);
        Gizmos.DrawIcon (enemyFormation.enemyPos9, "Number_09", false);
        Gizmos.DrawIcon (enemyFormation.enemyPos10, "Number_10", false);
        Gizmos.DrawIcon (enemyFormation.enemyPos11, "Number_11", false);
        Gizmos.DrawIcon (enemyFormation.enemyPos12, "Number_12", false);

        Gizmos.DrawIcon (unitFormation.unitPos1, "Number(1)G", false);
        Gizmos.DrawIcon (unitFormation.unitPos2, "Number(2)G", false);
        Gizmos.DrawIcon (unitFormation.unitPos3, "Number(3)G", false);
        Gizmos.DrawIcon (unitFormation.unitPos4, "Number(4)G", false);
        Gizmos.DrawIcon (unitFormation.unitPos5, "Number(5)G", false);
        Gizmos.DrawIcon (unitFormation.unitPos6, "Number(6)G", false);
        Gizmos.DrawIcon (unitFormation.unitPos7, "Number(7)G", false);
        Gizmos.DrawIcon (unitFormation.unitPos8, "Number(8)G", false);
        Gizmos.DrawIcon (unitFormation.unitPos9, "Number(9)G", false);
        Gizmos.DrawIcon (unitFormation.unitPos10, "Number(10)G", false);
        Gizmos.DrawIcon (unitFormation.unitPos11, "Number(11)G", false);
        Gizmos.DrawIcon (unitFormation.unitPos12, "Number(12)G", false);
    }
#endif
}
