using UnityEngine;

public class LeafBullet : Item
{
    private float spin = 150f;
    private float duration = 3f;

    private int amount = 3;
    private float shot = 10f;
    private float maxDistance = 8f;

    private Vector2 origin;
    private bool finished;

    protected override void Update()
    {
        base.Update();

        if (isActive)
        {
            transform.Rotate(0f, 0f, spin * Time.deltaTime);
            if (!finished && Vector2.Distance(transform.position, origin) >= maxDistance)
            {
                Stop();
                transform.localScale *= 1.5f;
                spin *= 2f;
                finished = true;
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D _collision)
    {
        base.OnTriggerEnter2D(_collision);

        if (_collision.CompareTag("Enemy") && isActive && !finished)
        {
            Stop();
            transform.localScale *= 1.5f;
            spin *= 2f;
            finished = true;
        }
    }

    public override void UseItem()
    {
        if (isActive) return;

        base.UseItem();

        origin = transform.position;

        var target = EntityManager.Instance.GetEnemyClosest((Vector3)transform.position);
        Vector2 dir = target != null
            ? ((Vector2)target.transform.position - (Vector2)transform.position).normalized
            : Vector2.up;

        Fire(dir);

        for (int i = 1; i < amount; i++)
        {
            var clone = Instantiate(gameObject, transform.position, Quaternion.identity, transform.parent);
            var lb = clone.GetComponent<LeafBullet>();
            lb.Activate(dir);
        }
    }

    private void Activate(Vector2 _dir)
    {
        if (isActive) return;
        base.UseItem();
        origin = transform.position;
        Fire(_dir);
    }

    private void Fire(Vector2 _dir)
    {
        Move(_dir * shot);
        EntityManager.Instance?.RemoveItem(this, duration);
    }
}
