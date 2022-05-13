using System.Collections.Generic;
using Tools.LevelCreator.Editor;
using Tools.LevelCreator.Runtime;
using UnityEditor;
using UnityEngine;

namespace Tools.LevelCreator.Editor
{
    [CustomEditor(typeof(Level))]
    public class LevelInspector : UnityEditor.Editor
    {
        private enum Mode
        {
            View,
            Paint,
            Edit,
            Erase
        }

        private PaletteItem itemSelected;
        private Texture2D itemPreview;
        private GameObject pieceSelected;
        private PaletteItem itemInspected;

        private Level myTarget;
        private int newTotalColumns;
        private int newTotalRows;

        private int originalPosX;
        private int originalPosY;

        private Mode selectedMode;
        private Mode currentMode;

        private void OnEnable()
        {
            myTarget = (Level)target;
            InitLevel();
            ResetResizeValues();
            SubscribeEvents();
        }


        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void UnSubscribeEvents()
        {
            PaletteWindow.ItemSelectedEvent -= UpdateCurrentPieceInstance;
        }

        private void SubscribeEvents()
        {
            PaletteWindow.ItemSelectedEvent += UpdateCurrentPieceInstance;
        }


        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("The GUI of this inspector was modified.");

            DrawLevelSizeGUI();
            DrawPieceSelectedGUI();
            DrawInspectedItemGUI();

            if (GUI.changed)
                EditorUtility.SetDirty(myTarget);
        }

        private void OnSceneGUI()
        {
            DrawModeGUI();
            ModeHandler();
            EventHandler();
        }

        private void EventHandler()
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            var camera = SceneView.currentDrawingSceneView.camera;
            var mousePosition = Event.current.mousePosition;
            mousePosition = new Vector2(mousePosition.x, camera.pixelHeight - mousePosition.y);

            var worldPos = camera.ScreenToWorldPoint(mousePosition);
            var gridPos = myTarget.WorldToGridCoordinates(worldPos);

            var col = (int)gridPos.x;
            var row = (int)gridPos.y;

            switch (currentMode)
            {
                case Mode.View:
                    break;
                case Mode.Paint:
                    if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
                        Paint(col, row);
                    break;
                case Mode.Edit:
                    if (Event.current.type == EventType.MouseDown)
                    {
                        Edit(col, row);
                        originalPosX = col;
                        originalPosY = row;
                    }

                    if (Event.current.type == EventType.MouseUp || Event.current.type == EventType.Ignore)
                    {
                        if (itemInspected != null)
                            Move();
                    }

                    if (itemInspected != null)
                    {
                        itemInspected.transform.position = Handles.FreeMoveHandle(itemInspected.transform.position,
                            itemInspected.transform.rotation, Level.GRID_SIZE / 2, Level.GRID_SIZE / 2 * Vector3.one,
                            Handles.RectangleHandleCap);
                    }

                    break;
                case Mode.Erase:
                    if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
                        Erase(col, row);
                    break;
                default:
                    break;
            }
        }

        private void Paint(int col, int row)
        {
            if (!myTarget.IsInsideGridBounds(col, row) || pieceSelected == null)
                return;

            if (myTarget.LevelPieces[col + row * myTarget.TotalColumns] != null)
                DestroyImmediate(myTarget.LevelPieces[col + row * myTarget.TotalColumns]);

            var obj = (GameObject)PrefabUtility.InstantiatePrefab(pieceSelected);
            obj.transform.parent = myTarget.transform;
            obj.name = $"[{col},{row},{obj.name}]";
            obj.transform.position = myTarget.GridToWorldCoordinates(col, row);
            obj.hideFlags = HideFlags.HideInHierarchy;
            myTarget.LevelPieces[col + row * myTarget.TotalColumns] = obj;
        }

        private void Erase(int col, int row)
        {
            if (!myTarget.IsInsideGridBounds(col, row))
                return;
            if (myTarget.LevelPieces[col + row * myTarget.TotalColumns] != null)
                DestroyImmediate(myTarget.LevelPieces[col + row * myTarget.TotalColumns]);
        }

        private void Edit(int col, int row)
        {
            if (!myTarget.IsInsideGridBounds(col, row) || myTarget.LevelPieces[col + row * myTarget.TotalColumns] == null)
                itemInspected = null;
            else
                itemInspected = myTarget.LevelPieces[col + row * myTarget.TotalColumns].GetComponent<PaletteItem>();

            Repaint();
        }

        private void Move()
        {
            var gridPoint = myTarget.WorldToGridCoordinates(itemInspected.transform.position);

            var col = (int)gridPoint.x;
            var row = (int)gridPoint.y;

            if (col == originalPosX && row == originalPosY)
                return;

            if (!myTarget.IsInsideGridBounds(col, row) || myTarget.LevelPieces[col + row * myTarget.TotalColumns] != null)
            {
                itemInspected.transform.position = myTarget.GridToWorldCoordinates(originalPosX, originalPosY);
            }
            else
            {
                myTarget.LevelPieces[originalPosX + originalPosY * myTarget.TotalColumns] = null;
                myTarget.LevelPieces[col + row * myTarget.TotalColumns] = itemInspected.gameObject;
                myTarget.LevelPieces[col + row * myTarget.TotalColumns].transform.position =
                    myTarget.GridToWorldCoordinates(col, row);
            }
        }

        private void DrawInspectedItemGUI()
        {
            if (currentMode != Mode.Edit)
                return;
            EditorGUILayout.LabelField("Piece Edited", EditorStyles.boldLabel);

            if (itemInspected != null)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("name: " + itemInspected.name);
                    // Editor.CreateEditor(itemSelected.inspectedScript).OnInspectorGUI();
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.HelpBox("No piece to edit!", MessageType.Info);
            }
        }

        private void DrawModeGUI()
        {
            var modes = EditorUtils.GetListFromEnum<Mode>();
            var modeLabels = new List<string>();

            foreach (var item in modes)
                modeLabels.Add(item.ToString());

            Handles.BeginGUI();
            {
                GUILayout.BeginArea(new Rect(10f, 10f, 340f, 40f));
                {
                    selectedMode = (Mode)GUILayout.Toolbar((int)currentMode, modeLabels.ToArray(),
                        GUILayout.ExpandHeight(true));
                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();
        }

        private void ModeHandler()
        {
            switch (selectedMode)
            {
                case Mode.View:
                    UnityEditor.Tools.current = Tool.View;
                    break;
                case Mode.Paint:
                case Mode.Edit:
                case Mode.Erase:
                    UnityEditor.Tools.current = Tool.None;
                    break;
                default:
                    UnityEditor.Tools.current = Tool.View;
                    break;
            }

            if (selectedMode != currentMode)
            {
                currentMode = selectedMode;
                itemSelected = null;
                Repaint();
            }

            SceneView.currentDrawingSceneView.in2DMode = true;
        }

        private void DrawPieceSelectedGUI()
        {
            EditorGUILayout.LabelField("Piece Selected", EditorStyles.boldLabel);
            if (pieceSelected == null)
            {
                EditorGUILayout.HelpBox("No Piece Selected!", MessageType.Info);
            }
            else
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField(new GUIContent(itemPreview), GUILayout.Height(40));
                    EditorGUILayout.LabelField(pieceSelected.name);
                }
                EditorGUILayout.EndVertical();
            }
        }

        private void UpdateCurrentPieceInstance(PaletteItem item, Texture2D preview)
        {
            itemSelected = item;
            itemPreview = preview;
            pieceSelected = item.gameObject;
            Repaint();
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
                myTarget.transform.hideFlags = HideFlags.NotEditable;
            }
        }

        private void ResetResizeValues()
        {
            newTotalColumns = myTarget.TotalColumns;
            newTotalRows = myTarget.TotalRows;
        }

        private void ResizeLevel()
        {
            var newPieces = new GameObject[newTotalColumns * newTotalRows];

            var targetCols = newTotalColumns < myTarget.TotalColumns ? newTotalColumns : myTarget.TotalColumns;
            var targetRows = newTotalRows < myTarget.TotalRows ? newTotalRows : myTarget.TotalRows;

            for (var col = 0; col < myTarget.TotalColumns; col++)
            {
                for (var row = 0; row < myTarget.TotalRows; row++)
                {
                    if (col < targetCols && row < targetRows)
                        newPieces[col + row * targetCols] = myTarget.LevelPieces[col + row * myTarget.TotalColumns];
                    else
                    {
                        var piece = myTarget.LevelPieces[col + row * myTarget.TotalColumns];
                        if (piece != null)
                            DestroyImmediate(piece);
                    }
                }
            }

            myTarget.LevelPieces = newPieces;
            myTarget.TotalColumns = newTotalColumns;
            myTarget.TotalRows = newTotalRows;
        }
    }
}