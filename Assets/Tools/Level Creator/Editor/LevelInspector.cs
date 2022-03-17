using UnityEngine;
using UnityEditor;
using Platformer;

[CustomEditor(typeof(Level))]
public class LevelInspector : Editor
{
    private Level myTarget;
    private int newTotalColumns;
    private int newTotalRows;


    private void OnEnable()
    {
        myTarget = (Level) target;
        InitLevel();
        ResetResizeValues();
    }

    private void OnDisable()
    {
    }

    private void OnDestroy()
    {
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("The GUI of this inspector was modified.");
        // DrawLevelDataGUI();
        DrawLevelSizeGUI();
        if (GUI.changed)
            EditorUtility.SetDirty(myTarget);
    }

    private void DrawLevelSizeGUI()
    {
        EditorGUILayout.LabelField("Size", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal("Box");
        {
            EditorGUILayout.BeginVertical();
            {
                newTotalColumns = EditorGUILayout.IntField("Columns", Mathf.Max(1, newTotalColumns));
                newTotalRows = EditorGUILayout.IntField("Rows", Mathf.Max(1, newTotalRows));
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            {
                var oldEnabled = GUI.enabled;
                GUI.enabled = (newTotalColumns != myTarget.TotalColumns || newTotalRows != myTarget.TotalRows);

                var buttonResize = GUILayout.Button("Resize", GUILayout.Height(2 * EditorGUIUtility.singleLineHeight));
                if (buttonResize)
                {
                    if (EditorUtility.DisplayDialog("Level Creator",
                        "Are you sure, you want to resize the level?\nthis action cannot be undone!", "Yes", "No"))
                    {
                        ResizeLevel();
                    }
                }

                var buttonRest = GUILayout.Button("Rest");
                if (buttonRest)
                {
                    ResetResizeValues();
                }

                GUI.enabled = oldEnabled;
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }


    private void InitLevel()
    {
        if (myTarget.LevelPieces == null || myTarget.LevelPieces.Length == 0)
        {
            myTarget.LevelPieces = new GameObject[myTarget.TotalColumns * myTarget.TotalRows];
        }
    }

    private void DrawLevelDataGUI()
    {
        EditorGUILayout.LabelField("Data", EditorStyles.boldLabel);
    }

    private void ResetResizeValues()
    {
        newTotalColumns = myTarget.TotalColumns;
        newTotalRows = myTarget.TotalRows;
    }

    private void ResizeLevel()
    {
        var newPieces = new GameObject[newTotalColumns * newTotalRows];

        for (var col = 0; col < myTarget.TotalColumns; col++)
        {
            for (var row = 0; row < myTarget.TotalRows; row++)
            {
                if (col < newTotalColumns && row < newTotalRows)
                    newPieces[col + row * newTotalColumns] = myTarget.LevelPieces[col + row * newTotalColumns];
                else
                {
                    var piece = myTarget.LevelPieces[col + row * newTotalColumns];
                    if (piece != null)
                        Object.DestroyImmediate(piece);
                }
            }
        }

        myTarget.LevelPieces = newPieces;
        myTarget.TotalColumns = newTotalColumns;
        myTarget.TotalRows = newTotalRows;
    }
}