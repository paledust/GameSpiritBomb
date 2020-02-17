using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HealthLevel))]
public class HealthEditor : Editor
{
    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
        HealthLevel healthLevel = target as HealthLevel;
        GUILayout.BeginHorizontal();
            GUILayout.Label("Health:");
            GUILayout.TextField(healthLevel.level.ToString());
        GUILayout.EndHorizontal();
    }
}
