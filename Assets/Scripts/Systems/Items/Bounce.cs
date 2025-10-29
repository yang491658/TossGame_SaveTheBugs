using UnityEngine;

public class Bounce : Item
{
    #region 스케일
    private float scale = 3f;
    private float spin = 30f;
    #endregion

    #region 능력
    private Player player;

    private float speedRatio = 5f;
    private float minSpeed = 5f;
    private Vector3 direction = Vector3.up;

    private float timer;
    private float duration = 20f;
    private int bounce = 5;
    #endregion

    protected override void Update()
    {
        base.Update();

        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer >= duration) bounce = 0;

            transform.Rotate(0f, 0f, spin * rb.linearVelocity.magnitude * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Background") && bounce > 0)
        {
            var wall = _collision.gameObject.name;
            if (wall.EndsWith("Top") || wall.EndsWith("Bottom"))
                rb.linearVelocityY *= -1;
            else if (wall.EndsWith("Left") || wall.EndsWith("Right"))
                rb.linearVelocityX *= -1;

            bounce--;
        }
    }

    public override void UseItem()
    {
        if (isActive) return;
        base.UseItem();

        Stop();

        transform.localScale *= scale;
        player = EntityManager.Instance?.GetPlayer();
        timer = 0f;

        SetDirection(player.transform.up);
        Fire();
    }

    private void Fire()
        => Move(direction * Mathf.Max(player.GetSpeed() * speedRatio, minSpeed));

    #region SET
    public void SetDirection(Vector3 _dir)
    {
        transform.up = _dir;
        direction = _dir;
    }
    #endregion
}
