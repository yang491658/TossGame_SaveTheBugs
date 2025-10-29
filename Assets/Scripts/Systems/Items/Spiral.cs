using UnityEngine;
using System.Collections;

public class Spiral : Item
{
    #region 스케일
    private float scale = 0.8f;
    #endregion

    #region 능력
    private Player player;

    private bool isOrigin = true;
    private int count = 12;
    private float angle = 30f;
    private float speed = 8f;
    private Vector3 direction = Vector3.up;
    private float delay = 0.05f;
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


            StartCoroutine(MakeClone());
        }
        else
        {
            transform.localScale *= scale;

            Fire();
        }
    }

    private IEnumerator MakeClone()
    {
        Vector3 baseDir = player.transform.up;
        float currentAngle = 0f;

        while (count > 0)
        {
            Vector3 dir = Quaternion.Euler(0f, 0f, currentAngle) * baseDir;

            Spiral clone = EntityManager.Instance.SpawnItem(data.ID, player.transform.position)
                .GetComponent<Spiral>();

            clone.SetClone();
            clone.SetDirection(dir);
            clone.UseItem();

            currentAngle += angle;
            count--;
            yield return new WaitForSeconds(delay);
        }

        EntityManager.Instance?.RemoveItem(this);
    }

    private void Fire() => Move(direction * speed);

    #region SET
    public void SetClone() => isOrigin = false;

    public void SetDirection(Vector3 _dir)
    {
        transform.up = _dir;
        direction = _dir;
    }
    #endregion
}

