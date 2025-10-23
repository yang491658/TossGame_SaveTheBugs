using UnityEngine;

public class Item : Entity
{
    private float speed = 3f;
    private int bounce = 0;

    protected override void Start()
    {
        base.Start();

        float sx = Random.value < 0.5f ? -1f : 1f;
        Vector2 dir = new Vector2(sx, -1f).normalized;
        Move(dir * speed);
    }

    protected override void Update()
    {
        base.Update();

        Debug.Log(gameObject.name + " : "+ rb.linearVelocity + " / " + bounce);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Background") && bounce < 3)
        {
            Bounds b = _collision.bounds;
            Vector2 p = transform.position;
            float dl = Mathf.Abs(p.x - b.min.x);
            float dr = Mathf.Abs(b.max.x - p.x);
            float db = Mathf.Abs(p.y - b.min.y);
            float dt = Mathf.Abs(b.max.y - p.y);
            float m = Mathf.Min(Mathf.Min(dl, dr), Mathf.Min(db, dt));

            Vector2 v = rb.linearVelocity;
            if (Mathf.Approximately(m, dl) || Mathf.Approximately(m, dr))
                v.x = -v.x;
            else
                v.y = -v.y;

            Move(v);
            bounce++;
        }
    }

    private void OnBecameInvisible()
    {
        EntityManager.Instance?.Remove(this);
    }

    public virtual void UseItem()
    {
        EntityManager.Instance?.Remove(this);
    }
}
