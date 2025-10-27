using UnityEngine;

public class Missile : Item
{
    #region ������
    private float scale = 3.5f;
    #endregion

    #region �ɷ�
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
