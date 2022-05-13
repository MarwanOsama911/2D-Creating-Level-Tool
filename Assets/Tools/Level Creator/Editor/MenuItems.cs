using UnityEditor;

namespace Tools.LevelCreator.Editor
{
    public static class MenuItems
    {
        [MenuItem("Tools/Level Creator/New Level Scene")]
        public static void NewLevel()
        {
            EditorUtils.NewLevel();
        }

        [MenuItem("Tools/Level Creator/Show Palette #P")]
        public static void ShowPalette()
        {
            PaletteWindow.ShowPalette();
        }
    }
}