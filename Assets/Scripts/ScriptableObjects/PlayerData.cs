using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "EntityData/Player", order = 1)]
public class PlayerData : EntityData
{
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        ID = 0;
        Name = "Player";
    }
#endif
    
    public override EntityData Clone()
    {
        var clone = CreateInstance<PlayerData>();
        clone.ID = this.ID;
        clone.Name = this.Name;
        clone.Image = this.Image;
        return clone;
    }
}
