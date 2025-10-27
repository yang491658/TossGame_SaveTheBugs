using UnityEngine;
using System.Collections;

public class Bullet : Item
{
    #region 스케일
    private float scale = 0.5f;
    #endregion
    #region 능력
    private bool isOrigin = true;
    private int count = 10;
    private float shot = 20f;
    private Vector3 direction = Vector3.up;
    private float delay = 0.3f;
    #endregion

    public override void UseItem()
    {
        if (isActive) return;
        base.UseItem();

        if (isOrigin)
        {
            Stop();

            transform.SetParent(EntityManager.Instance?.GetPlayer().transform);
            transform.localPosition = Vector3.zero;
            transform.localScale *= scale;

            rb.bodyType = RigidbodyType2D.Kinematic;

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
            Bullet clone = EntityManager.Instance.SpawnItem(data.ID, transform.position)
                .GetComponent<Bullet>();

            clone.SetClone();
            //clone.SetDirection(direction);
            clone.UseItem();

            count--;
            yield return new WaitForSeconds(delay);
        }

        EntityManager.Instance?.RemoveItem(this);
    }

    private void Fire() => Move(direction * shot);

    #region SET
    public void SetClone() => isOrigin = false;
    public void SetDirection(Vector3 _dir) => direction = _dir;
    #endregion
}
