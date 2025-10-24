using UnityEngine;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Item", menuName = "ItemData", order = 2)]
public class ItemData : EntityData
{
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        var sprites = Resources.LoadAll<Sprite>("Images/Items");
        var used = new System.Collections.Generic.HashSet<string>();
        foreach (var g in AssetDatabase.FindAssets("t:ItemData"))
        {
            var d = AssetDatabase.LoadAssetAtPath<ItemData>(AssetDatabase.GUIDToAssetPath(g));
            if (d != null && d != this && d.Image != null)
                used.Add(d.Image.name);
        }

        Sprite pick = null;
        if (Image == null || used.Contains(Image.name))
        {
            foreach (var s in sprites)
            {
                if (!used.Contains(s.name)) { pick = s; break; }
            }
            Image = pick;
        }

        if (Image != null)
        {
            var m = Regex.Match(Image.name, @"^(?<num>\d+)\.");
            ID = m.Success ? int.Parse(m.Groups["num"].Value) : ID;
        }
        else ID = 0;

        base.OnValidate();
        EditorUtility.SetDirty(this);
    }
#endif

    public override EntityData Clone()
    {
        var clone = CreateInstance<ItemData>();
        clone.ID = this.ID;
        clone.Name = this.Name;
        clone.Image = this.Image;
        return clone;
    }
}
