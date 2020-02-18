using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Cell))]
public class CellEditor : Editor
{
    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
        Cell cell = target as Cell;

        GUILayout.Label("Location Index",EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
            GUILayout.Label("X:");
            GUILayout.TextField(cell.index.x.ToString());
            GUILayout.Label("Y:");
            GUILayout.TextField(cell.index.y.ToString());
        GUILayout.EndHorizontal();
    }
}
[CustomEditor(typeof(Cell_Red))]
public class Cell_Red_Editor : Editor
{
    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
        Cell cell = target as Cell;

        GUILayout.Label("Location Index",EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
            GUILayout.Label("X:");
            GUILayout.TextField(cell.index.x.ToString());
            GUILayout.Label("Y:");
            GUILayout.TextField(cell.index.y.ToString());
        GUILayout.EndHorizontal();
    }
}
[CustomEditor(typeof(Cell_Grey))]
public class Cell_Grey_Editor : Editor
{
    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
        Cell cell = target as Cell;

        GUILayout.Label("Location Index",EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
            GUILayout.Label("X:");
            GUILayout.TextField(cell.index.x.ToString());
            GUILayout.Label("Y:");
            GUILayout.TextField(cell.index.y.ToString());
        GUILayout.EndHorizontal();
    }
}

