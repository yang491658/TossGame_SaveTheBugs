using UnityEngine;

public class Bounce : Item
{
    #region 스케일
    private float scale = 3f;
    #endregion

    #region 능력
    private Player player;
    private float speedRatio = 5f;
    private Vector2 speedRange = new Vector2(3f, 30f);
    private Vector3 direction = Vector3.up;
    private float duration = 20f;
    private int bounce = 5;
    #endregion

    protected override void Update()
    {
        base.Update();

        if (isActive)
            transform.Rotate(0f, 0f, rb.linearVelocity.magnitude * Time.deltaTime);
    }

    public override void UseItem()
    {
        if (isActive) return;
        base.UseItem();

        Stop();

        player = EntityManager.Instance?.GetPlayer();
        transform.localScale *= scale;

        SetDirection(player.transform.up);
        Fire();
        EntityManager.Instance?.RemoveItem(this, duration);
    }

    private void Fire()
    => Move(direction * Mathf.Clamp(player.GetSpeed() * speedRatio, speedRange.x, speedRange.y));

    #region SET
    public void SetDirection(Vector3 _dir)
    {
        transform.up = _dir;
        direction = _dir;
    }
    #endregion
}
