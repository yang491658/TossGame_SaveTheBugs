﻿using UnityEngine;

public class Item : Entity
{
    public bool isActive { private set; get; } = false;

    private float speed = 3.5f;

    private float timer = 0f;
    private float delay = 15f;
    private Collider2D backCol;

    protected override void Awake()
    {
        base.Awake();

        backCol = transform.Find("Background")?.GetComponent<Collider2D>();
    }

    protected override void Start()
    {
        base.Start();

        if (!isActive)
        {
            float angle = Random.Range(0f, 360f);
            float rad = angle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
            Move(dir * speed);
        }
    }

    protected override void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay && backCol != null)
            backCol.isTrigger = true;
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

        sr.sortingOrder = ((ItemData)data).Sort;
        col.isTrigger = true;
        isActive = true;
        if (backCol != null) Destroy(backCol.gameObject);
    }
}
