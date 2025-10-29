using UnityEngine;

public class Barrier : Item
{
    #region 스케일
    private float scale = 2f;
    private float spin = 120f;
    #endregion

    #region 능력
    private Player player;

    private float duration = 10f;
    #endregion

    protected override void Update()
    {
        base.Update();

        if (isActive)
            transform.Rotate(0f, 0f, spin * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (isActive)
            transform.position = player.transform.position;
    }

    public override void UseItem()
    {
        if (isActive) return;
        base.UseItem();

        transform.localScale *= scale;
        rb.bodyType = RigidbodyType2D.Kinematic;
        player = EntityManager.Instance?.GetPlayer();

        Stop();
        EntityManager.Instance?.RemoveItem(this, duration);
    }
}
