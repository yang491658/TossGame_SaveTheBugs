using UnityEngine;

public class Item : Entity
{
    public bool isActive { private set; get; } = false;

    private float speed = 5f;

    private float timer = 0f;
    private float delay = 15f;

    protected override void Awake()
    {
        base.Awake();

        col.isTrigger = false;
    }

    protected override void Start()
    {
        base.Start();

        float angle = Random.Range(0f, 360f);
        float rad = angle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        Move(dir * speed);
    }

    protected override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer > delay) col.isTrigger = true;
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

    public virtual void UseItem()
    {
        if (isActive) return;

        Debug.Log(gameObject.name + " 발동");

        sr.sortingOrder = -1;
        col.isTrigger = true;
        isActive = true;
        Destroy(transform.Find("Background").gameObject);
    }
}
