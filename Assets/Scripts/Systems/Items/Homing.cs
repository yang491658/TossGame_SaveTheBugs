using UnityEngine;
using System.Collections;

public class Homing : Item
{
    #region 스케일
    private float scale = 3f;
    private float spin = 180f;
    #endregion

    #region 능력
    private bool isOrigin = true;
    private int count = 3;
    private float angle = 90f;

    private bool isMoving = true;
    private float shot = 10f;
    private Vector3 direction = Vector3.up;
    private Vector3 basePos;
    private float minDistance = 5f;
    private float duration = 3.5f;

    private bool isHoming = false;
    private Enemy target;
    #endregion
    protected override void Update()
    {
        base.Update();

        if (isActive)
            transform.Rotate(0f, 0f, spin * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Enemy") && isActive && isMoving)
        {
            if ((transform.position - basePos).sqrMagnitude > (minDistance * minDistance))
            {
                Stop();

                transform.localScale *= scale;
                spin *= scale;
                isMoving = false;

                EntityManager.Instance?.RemoveItem(this, duration);
            }
        }
    }

    public override void UseItem()
    {
        if (isActive) return;
        base.UseItem();

        basePos = transform.position;

        if (isOrigin)
        {
            int cloneCount = count - 1;
            float start = -angle * 0.5f;
            float step = cloneCount <= 0 ? 0f : angle / (count - 1);

            direction = SetRotate(direction, start);

            for (int i = 1; i <= cloneCount; i++)
            {
                float deg = start + step * i;
                Vector3 dir = SetRotate(direction, deg - start);

                Homing clone = EntityManager.Instance.SpawnItem(data.ID, transform.position)
                    .GetComponent<Homing>();

                clone.SetClone();
                clone.SetDirection(dir);
                clone.UseItem();
            }
        }

        StartCoroutine(Chase());
    }

    private IEnumerator Chase()
    {
        while (isMoving)
        {
            if (!isHoming)
                if ((transform.position - basePos).sqrMagnitude >= (minDistance * minDistance))
                    isHoming = true;

            if (isHoming)
            {
                if (target == null)
                    target = EntityManager.Instance.GetEnemyClosest(transform.position);

                if (target != null)
                    direction = (target.transform.position - transform.position).normalized;
            }

            if (direction == Vector3.zero)
                direction = Vector3.up;

            Move(direction * shot);

            yield return null;
        }
    }

    #region SET
    public void SetClone() => isOrigin = false;
    public void SetDirection(Vector3 _dir) => direction = _dir;
    private Vector3 SetRotate(Vector3 _dir, float _deg)
    {
        float r = _deg * Mathf.Deg2Rad;
        float cs = Mathf.Cos(r);
        float sn = Mathf.Sin(r);
        return new Vector3(_dir.x * cs - _dir.y * sn, _dir.x * sn + _dir.y * cs, 0f).normalized;
    }
    #endregion
}
