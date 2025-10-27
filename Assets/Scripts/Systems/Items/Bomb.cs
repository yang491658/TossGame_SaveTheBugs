using UnityEngine;

public class Bomb : Item
{
    #region ������
    private float scale = 5f;
    private float spin = 30f;
    #endregion

    #region �ɷ�
    private float duration = 10f;
    #endregion

    protected override void Update()
    {
        base.Update();

        if (isActive)
            transform.Rotate(0f, 0f, spin * Time.deltaTime);
    }

    public override void UseItem()
    {
        if (isActive) return;
        base.UseItem();

        Stop();

        transform.localScale *= scale;

        EntityManager.Instance?.RemoveItem(this, duration);
    }
}
