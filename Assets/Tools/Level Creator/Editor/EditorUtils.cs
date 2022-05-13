using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Tools.LevelCreator.Runtime;
using UnityEditor.SceneManagement;

namespace Tools.LevelCreator.Editor
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

        public static List<T> GetAssetsWithScript<T>(string path) where T : MonoBehaviour
        {
            T temp;
            string assetsPath;
            GameObject asset;

            var assetsList = new List<T>();
            var guids = AssetDatabase.FindAssets("t:Prefab", new string[] {path});

            foreach (var item in guids)
            {
                assetsPath = AssetDatabase.GUIDToAssetPath(item);
                asset = AssetDatabase.LoadAssetAtPath(assetsPath, typeof(GameObject)) as GameObject;
                temp = asset.GetComponent<T>();
                if (temp != null)
                    assetsList.Add(temp);
            }

            return assetsList;
        }

        public static List<T> GetListFromEnum<T>()
        {
            var enumList = new List<T>();
            var enums = System.Enum.GetValues(typeof(T));
            foreach (T item in enums)
                enumList.Add(item);

            return enumList;
        }
    }
}