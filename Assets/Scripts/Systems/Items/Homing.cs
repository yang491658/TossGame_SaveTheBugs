using System.Collections;
using UnityEngine;

public class Homing : Item
{
    #region 스케일
    private float scale = 2.5f;
    private float spin = 360f;
    #endregion

    #region 능력
    private Player player;
    private Enemy target;

    private bool isOrigin = true;
    private bool isMoving = true;
    private bool isHoming = false;

    private int count = 3;
    private float angle = 90f;
    private float speed = 10f;
    private Vector3 direction = Vector3.up;

    private Vector3 basePos;
    private float distance = 5f;
    private float duration = 3.5f;
    #endregion

    protected override void Update()
    {
        base.Update();

        if (isActive)
            transform.Rotate(0f, 0f, spin * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Enemy") && isActive && !isOrigin && isMoving)
        {
            if ((transform.position - basePos).sqrMagnitude > (distance * distance))
            {
                transform.localScale *= scale;
                sr.sortingOrder = 1;
                spin *= scale;
                isMoving = false;

                Stop();
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
            CopySelf();
            EntityManager.Instance?.RemoveItem(this);
        }
        else StartCoroutine(ChaseCoroutine());
    }

    private void CopySelf()
    {
        player = EntityManager.Instance?.GetPlayer();

        float start = -angle * 0.5f;
        float step = (count - 1) > 0 ? angle / (count - 1) : 0f;
        
        SetDirection(SetRotate(direction, start));

        for (int i = 0; i < count; i++)
        {
            float deg = start + step * i;
            Vector3 dir = SetRotate(direction, deg - start);

            Homing copy = EntityManager.Instance.SpawnItem(data.ID, player.transform.position)
                .GetComponent<Homing>();

            copy.SetClone();
            copy.SetDirection(dir);
            copy.UseItem();
        }
    }

    private IEnumerator ChaseCoroutine()
    {
        while (isMoving)
        {
            if (!isHoming)
                if ((transform.position - basePos).sqrMagnitude >= (distance * distance))
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

            Fire();

            yield return null;
        }
    }

    private void Fire()
        => Move(direction * speed);

    #region SET
    public void SetClone() => isOrigin = false;
    public Vector3 SetRotate(Vector3 _dir, float _deg)
    {
        float r = _deg * Mathf.Deg2Rad;
        float cs = Mathf.Cos(r);
        float sn = Mathf.Sin(r);
        return new Vector3(_dir.x * cs - _dir.y * sn, _dir.x * sn + _dir.y * cs, 0f).normalized;
    }
    public void SetDirection(Vector3 _dir)
    {
        transform.up = _dir;
        direction = _dir;
    }
    #endregion
}
