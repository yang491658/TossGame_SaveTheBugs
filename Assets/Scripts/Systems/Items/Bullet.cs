using System.Collections;
using UnityEngine;

public class Bullet : Item
{
    #region 스케일
    private float scale = 0.8f;
    #endregion

    #region 능력
    private Player player;

    private bool isOrigin = true;
    private int count = 10;
    private float speedRatio = 3f;
    private float minSpeed = 1f;
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

        player = EntityManager.Instance?.GetPlayer();

        if (isOrigin)
        {
            transform.position = player.transform.position;
            sr.color = new Color(1f, 1f, 1f, 0f);
            StartCoroutine(CopySelf());
        }
        else
        {
            transform.localScale *= scale;
            Fire();
        }
    }

    private IEnumerator CopySelf()
    {
        for (int i = 0; i < count; i++)
        {
            Bullet copy = EntityManager.Instance.SpawnItem(data.ID, player.transform.position)
                .GetComponent<Bullet>();

            copy.SetClone();
            copy.SetDirection(player.transform.up);
            copy.UseItem();

            yield return new WaitForSeconds(delay);
        }

        EntityManager.Instance?.RemoveItem(this);
    }

    private void Fire()
        => Move(direction * Mathf.Max(player.GetSpeed() * speedRatio, minSpeed));

    #region SET
    public void SetClone() => isOrigin = false;
    public void SetDirection(Vector3 _dir)
    {
        transform.up = _dir;
        direction = _dir;
    }
    #endregion
}
