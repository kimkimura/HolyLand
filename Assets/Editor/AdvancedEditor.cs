using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class AdvancedEditor : Editor {
    public override void OnInspectorGUI()
    {
        UnitFormation unitFormation = target as UnitFormation;
    }
}
