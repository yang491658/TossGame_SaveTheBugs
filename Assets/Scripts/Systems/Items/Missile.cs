using UnityEngine;

public class Missile : Item
{
    #region 스케일
    private float scale = 3f;
    #endregion

    #region 능력
    private float speed = 10f;
    #endregion

    public override void UseItem()
    {
        if (isActive) return;
        base.UseItem();

        Move(Vector3.up *speed);

        transform.localScale *= scale;
    }
}
