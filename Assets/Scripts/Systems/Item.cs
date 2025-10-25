using UnityEngine;

public class Item : Entity
{
    private float speed = 3.5f;
    private int bounce = 0;
    public bool isActive { private set; get; } = false;

    protected override void Start()
    {
        base.Start();

        float angle = Random.Range(180f, 360f);
        float rad = angle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        Move(dir * speed);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Background") && bounce < 5)
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

        if (_collision.CompareTag("Enemy") && isActive)
            EntityManager.Instance?.RemoveItem(_collision.GetComponent<Item>());
    }

    private void OnBecameInvisible()
    {
        EntityManager.Instance?.RemoveItem(this);
    }

    public virtual void UseItem()
    {
        if (isActive) return;
        
        Debug.Log(gameObject.name + " 발동");

        isActive = true;
    }
}
