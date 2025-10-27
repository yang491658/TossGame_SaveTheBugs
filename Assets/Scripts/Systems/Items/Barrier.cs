using UnityEngine;

public class Barrier : Item
{
    #region 스케일
    private float scale = 2.5f;
    private float spin = 120f;
    #endregion

    #region 능력
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
        
        transform.SetParent(EntityManager.Instance?.GetPlayer().transform);
        transform.localPosition = Vector3.zero;
        transform.localScale *= scale;

        rb.bodyType = RigidbodyType2D.Kinematic;

        EntityManager.Instance?.RemoveItem(this, duration);
    }
}
