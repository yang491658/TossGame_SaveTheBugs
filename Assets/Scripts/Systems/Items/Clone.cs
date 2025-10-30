using UnityEngine;

public class Clone : Item
{
    #region ������
    [Header("Scale")]
    [SerializeField] private float scale = 1f;
    #endregion

    #region �ɷ�
    [Header("Ability")]
    [SerializeField] private float speed = 8f;
    #endregion

    public override void UseItem()
    {
        if (isActive) return;
        base.UseItem();

        transform.localScale *= scale;
        Fire();
    }

    private void Fire()
        => Move(Vector3.up * speed);
}
