#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(EnemyData))]
public class EnemyDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnemyData data = (EnemyData)target;

        string idStr = data.ID.ToString("D2");
        string newName = $"Enemy{idStr}_{data.Name}";

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
