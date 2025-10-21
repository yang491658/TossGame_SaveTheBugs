using UnityEngine;

public class Enemy : Entity
{
    private float speed = 3f;

    protected override void Start()
    {
        base.Start();

        var p = EntityManager.Instance?.GetPlayer();

        Vector2 dir = (p.transform.position - transform.position).normalized;
        Move(dir * speed);
    }

    private void OnBecameInvisible()
    {
        EntityManager.Instance?.Despawn(this);
    }
}
