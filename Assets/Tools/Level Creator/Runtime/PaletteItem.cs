using UnityEngine;

public class PaletteItem : MonoBehaviour
{
#if UNITY_EDITOR
    public enum Category
    {
        Background,
        Collectables,
        Player,
        Diffs,
        Spikes
    }

    public Category category = Category.Background;
    public string itemName = "";
#endif
}