using System.Collections.Generic;
using Tools.LevelCreator.Editor;
using UnityEditor;
using UnityEngine;

namespace Tools.LevelCreator.Editor
{
    public class PaletteWindow : EditorWindow
    {
        public static PaletteWindow Instance;

        public delegate void ItemSelectedDelegate(PaletteItem item, Texture2D preview);
        public static event ItemSelectedDelegate ItemSelectedEvent;


        private List<PaletteItem.Category> categories;
        private List<string> categoryLabels;
        private PaletteItem.Category categorySelected;


        private string path = "Assets/Prefabs/Level Pieces";
        private List<PaletteItem> _items;
        private Dictionary<PaletteItem.Category, List<PaletteItem>> _categorizedItems;
        private Dictionary<PaletteItem, Texture2D> _previews;
        private Vector2 _scrollPosition;

        private const float BUTTON_WIDTH = 80f;
        private const float BUTTON_HIGHT = 90f;


        private void OnEnable()
        {
            if (categories == null)
                InitCategories();
            if (_categorizedItems == null)
                InitContent();
        }

        private void OnGUI()
        {
            DrawTabs();
            DrawScroll();
        }

        public static void ShowPalette()
        {
            Instance = (PaletteWindow) EditorWindow.GetWindow<PaletteWindow>();
            Instance.titleContent = new GUIContent("Palette");
        }

        private void InitContent()
        {
            _items = EditorUtils.GetAssetsWithScript<PaletteItem>(path);
            _categorizedItems = new Dictionary<PaletteItem.Category, List<PaletteItem>>();
            _previews = new Dictionary<PaletteItem, Texture2D>();

            foreach (PaletteItem.Category category in categories)
                _categorizedItems.Add(category, new List<PaletteItem>());

            foreach (PaletteItem item in _items)
                _categorizedItems[item.category].Add(item);
        }

        private void InitCategories()
        {
            categories = EditorUtils.GetListFromEnum<PaletteItem.Category>();
            categoryLabels = new List<string>();
            foreach (var item in categories)
                categoryLabels.Add(item.ToString());
        }

        private void DrawTabs()
        {
            var index = (int) categorySelected;
            index = GUILayout.Toolbar(index, categoryLabels.ToArray());
            categorySelected = categories[index];
        }

        private void DrawScroll()
        {
            if (_categorizedItems[categorySelected].Count == 0)
            {
                EditorGUILayout.HelpBox("This Category is empty!!", MessageType.Info);
                return;
            }

            var rowCapacity = Mathf.FloorToInt(position.width / (BUTTON_WIDTH));
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

            var selectionGridIndex = -1;
            selectionGridIndex = GUILayout.SelectionGrid(selectionGridIndex, GetGUIContentsFromItems(),
                rowCapacity, GetGUIStyle());
            GetSelectedItem(selectionGridIndex);
            GUILayout.EndScrollView();
        }

        private void GeneratePreview()
        {
            foreach (PaletteItem item in _items)
            {
                if (!_previews.ContainsKey(item))
                {
                    var preview = AssetPreview.GetAssetPreview(item.gameObject);
                    if (preview != null)
                        _previews.Add(item, preview);
                }
            }
        }

        private void GetSelectedItem(int index)
        {
            if (index != -1)
            {
                var selectedItem = _categorizedItems[categorySelected][index];
                Debug.Log("Selected Item is : " + selectedItem.itemName);

                if (ItemSelectedEvent != null)
                    ItemSelectedEvent.Invoke(selectedItem, _previews[selectedItem]);
            }
        }

        private GUIStyle GetGUIStyle()
        {
            var guiStyle = new GUIStyle(GUI.skin.button);
            guiStyle.alignment = TextAnchor.LowerCenter;
            guiStyle.imagePosition = ImagePosition.ImageAbove;
            guiStyle.fixedWidth = BUTTON_WIDTH;
            guiStyle.fixedHeight = BUTTON_HIGHT;

            return guiStyle;
        }

        private GUIContent[] GetGUIContentsFromItems()
        {
            var guiContents = new List<GUIContent>();
            if (_previews.Count == _items.Count)
            {
                var totalItems = _categorizedItems[categorySelected].Count;
                for (var i = 0; i < totalItems; i++)
                {
                    var guiContent = new GUIContent
                    {
                        text = _categorizedItems[categorySelected][i].itemName,
                        image = _previews[_categorizedItems[categorySelected][i]]
                    };
                    guiContents.Add(guiContent);
                }
            }

            return guiContents.ToArray();
        }


        private void Update()
        {
            if (_previews.Count != _items.Count)
                GeneratePreview();
        }
    }
}