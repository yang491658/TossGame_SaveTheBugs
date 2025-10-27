using UnityEngine;
using System.Collections;

public class Bullet : Item
{
    #region 스케일
    private float scale = 0.5f;
    #endregion

    #region 능력
    private Player player;
    private bool isOrigin = true;
    private int count = 10;
    private float ratio = 3f;
    private Vector2 speed = new Vector2(1f, 10f);
    private Vector3 direction = Vector3.up;
    private float delay = 0.3f;
    #endregion

    private void LateUpdate()
    {
        if (isActive && isOrigin && player != null)
            transform.position = player.transform.position;
    }

    public override void UseItem()
    {
        if (isActive) return;
        base.UseItem();

        Stop();

        player = EntityManager.Instance?.GetPlayer();

        if (isOrigin)
        {
            transform.position = player.transform.position;
            sr.color = new Color(1f, 1f, 1f, 0f);

            StartCoroutine(FireCoroutine());
        }
        else
        {
            transform.localScale *= scale;

            Fire();
        }
    }

    private IEnumerator FireCoroutine()
    {
        while (count > 0)
        {
            Bullet clone = EntityManager.Instance.SpawnItem(data.ID, player.transform.position)
                .GetComponent<Bullet>();

            clone.SetClone();
            clone.SetDirection(player.transform.up);
            clone.UseItem();

            count--;
            yield return new WaitForSeconds(delay);
        }

        EntityManager.Instance?.RemoveItem(this);
    }

    private void Fire()
    => Move(direction * Mathf.Clamp(player.GetSpeed() * ratio, speed.x, speed.y));

    #region SET
    public void SetClone() => isOrigin = false;

    public void SetDirection(Vector3 _dir)
    {
        transform.up = _dir;
        direction = _dir;
    }
    #endregion
}
