using UnityEngine;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Enemy", menuName = "EntityData/Enemy", order = 2)]
public class EnemyData : EntityData
{
    public int Score;

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        var sprites = Resources.LoadAll<Sprite>("Images/Enemies");
        var used = new System.Collections.Generic.HashSet<string>();
        foreach (var g in AssetDatabase.FindAssets("t:EnemyData"))
        {
            var d = AssetDatabase.LoadAssetAtPath<EnemyData>(AssetDatabase.GUIDToAssetPath(g));
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

        Score = ID * (ID + 1) / 2;

        base.OnValidate();
        EditorUtility.SetDirty(this);
    }
#endif

    public override EntityData Clone()
    {
        var clone = CreateInstance<EnemyData>();
        clone.ID = this.ID;
        clone.Name = this.Name;
        clone.Image = this.Image;
        clone.Score = this.Score;
        return clone;
    }
}
