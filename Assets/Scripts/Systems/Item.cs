using UnityEngine;
using System.Collections;

public class Item : Entity
{
    public bool isActive { private set; get; } = false;

    private float speed = 5f;
    private float delay = 15f;

    private Coroutine useRoutine;

    protected override void Start()
    {
        base.Start();

        float angle = Random.Range(0f, 360f);
        float rad = angle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        Move(dir * speed);

        useRoutine = StartCoroutine(UseCoroutine());
    }

    protected virtual void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Background") && !isActive)
        {
            Bounds b = _collision.bounds;
            Bounds mb = GetComponent<Collider2D>().bounds;

            Vector2 p = transform.position;
            float dl = Mathf.Abs(p.x - b.min.x);
            float dr = Mathf.Abs(b.max.x - p.x);
            float db = Mathf.Abs(p.y - b.min.y);
            float dt = Mathf.Abs(b.max.y - p.y);
            float m = Mathf.Min(Mathf.Min(dl, dr), Mathf.Min(db, dt));

            Vector2 v = rb.linearVelocity;

            if (Mathf.Approximately(m, dl))
            {
                v.x = -v.x;
                p.x = b.min.x + mb.extents.x;
            }
            else if (Mathf.Approximately(m, dr))
            {
                v.x = -v.x;
                p.x = b.max.x - mb.extents.x;
            }
            else if (Mathf.Approximately(m, db))
            {
                v.y = -v.y;
                p.y = b.min.y + mb.extents.y;
            }
            else
            {
                v.y = -v.y;
                p.y = b.max.y - mb.extents.y;
            }

            transform.position = p;
            Move(v);
        }
    }

    private void OnTriggerStay2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Enemy") && isActive)
        {
            GameManager.Instance?.AddScore();
            EntityManager.Instance?.RemoveEnemy(_collision.GetComponent<Enemy>());
        }
    }

    private void OnBecameInvisible()
    {
        EntityManager.Instance?.RemoveItem(this);
    }

    private IEnumerator UseCoroutine()
    {
        yield return new WaitForSeconds(delay);
        if (!isActive) UseItem();
    }

    public virtual void UseItem()
    {
        if (isActive) return;

        Debug.Log(gameObject.name + " 발동");

        if (useRoutine != null)
        {
            StopCoroutine(useRoutine);
            useRoutine = null;
        }

        isActive = true;
        Destroy(transform.Find("Background").gameObject);
    }
}
