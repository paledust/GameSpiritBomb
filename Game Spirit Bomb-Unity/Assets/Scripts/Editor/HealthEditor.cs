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

[CustomEditor(typeof(InfectionLevel))]
public class InfectionEditor : Editor
{
    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
        InfectionLevel infectionLevel = target as InfectionLevel;
        GUILayout.BeginHorizontal();
            GUILayout.Label("infection:");
            GUILayout.TextField(infectionLevel.level.ToString());
        GUILayout.EndHorizontal();
    }
}
