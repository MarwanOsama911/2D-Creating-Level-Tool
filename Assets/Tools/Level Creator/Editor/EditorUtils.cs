using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Platformer;
using UnityEditor.SceneManagement;

namespace LevelCreatorToolEditor
{
    public static class EditorUtils
    {
        public static void NewScene()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        public static void CleanScene()
        {
            var allObjects = Object.FindObjectsOfType<GameObject>();

            foreach (var item in allObjects)
            {
                Object.DestroyImmediate(item);
            }
        }

        public static void NewLevel()
        {
            NewScene();
            CleanScene();
            var levelObject = new GameObject("Level");
            levelObject.transform.position = Vector3.zero;
            levelObject.AddComponent<Level>();
        }
    }
}