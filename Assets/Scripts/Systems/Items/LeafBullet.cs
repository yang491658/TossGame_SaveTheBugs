using UnityEngine;

public class LeafBullet : Item
{
    private float spin = 150f;
    private float duration = 3f;

    private bool isOrigin = true;
    private int amount = 3;

    private bool isMoving = true;
    private float shot = 10f;
    private Vector3 origin;
    private float minDistance = 3f;

    protected override void Update()
    {
        base.Update();
        if (isActive) transform.Rotate(0f, 0f, spin * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Enemy") && isActive && isMoving)
        {
            if ((transform.position - origin).magnitude > minDistance)
            {
                Stop();
                transform.localScale *= 1.5f;
                spin *= 2f;
                isMoving = false;
            }
        }
    }

    public override void UseItem()
    {
        if (isActive) return;

        origin = transform.position;

        if (isOrigin)
        {
            for (int i = 1; i < amount; i++)
            {
                Instantiate(gameObject, transform.position, Quaternion.identity, transform.parent)
                   .GetComponent<LeafBullet>()
                   .SetClone();
            }
        }

        base.UseItem();
        Fire();
    }

    private void Fire()
    {
        Enemy target = EntityManager.Instance.GetEnemyRandom();
        Vector3 dir = target != null
            ? (target.transform.position - transform.position).normalized
            : Vector3.up;

        Move(dir * shot);
        EntityManager.Instance?.RemoveItem(this, duration);
    }

    public void SetClone() => isOrigin = false;
}
