using UnityEngine;
using System.Text.RegularExpressions;

[CreateAssetMenu(fileName = "Entity", menuName = "EntityData", order = 1)]
public class EntityData : ScriptableObject
{
    public int ID;
    public string Name;
    public Sprite Image;

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if (Image != null)
        {
            string rawName = Image.name;
            Name = Regex.Replace(rawName, @"^\d+\.", "");
        }
        else Name = null;
    }
#endif

    public virtual EntityData Clone()
    {
        EntityData clone = CreateInstance<EntityData>();
        clone.ID = this.ID;
        clone.Name = this.Name;
        clone.Image = this.Image;
        return clone;
    }
}
