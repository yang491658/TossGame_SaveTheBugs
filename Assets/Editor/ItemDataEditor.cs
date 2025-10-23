#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ItemData data = (ItemData)target;

        string idStr = data.ID.ToString("D2");
        string newName = $"Item{idStr}_{data.Name}";

        string path = AssetDatabase.GetAssetPath(data);
        string currentName = System.IO.Path.GetFileNameWithoutExtension(path);

        if (currentName != newName)
        {
            AssetDatabase.RenameAsset(path, newName);
            AssetDatabase.SaveAssets();
        }
    }
}
#endif
