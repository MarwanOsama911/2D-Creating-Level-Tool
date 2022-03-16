using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LevelCreatorToolEditor
{
    public static class MenuItems
    {
        [MenuItem("Tool/Level Creator/New Level Scene")]
        public static void NewLevel()
        {
            EditorUtils.NewLevel();
        }
    }
}