using UnityEngine;

public class Barrier : Item
{
    private float spin = 120f;
    private float duration = 10f;
    private float scale = 2.5f;

    protected override void Update()
    {
        base.Update();

        if (isActive)
            transform.Rotate(0f, 0f, spin * Time.deltaTime);
    }

    public override void UseItem()
    {
        if (isActive) return;

        base.UseItem();

        Stop();
        
        transform.SetParent(EntityManager.Instance?.GetPlayer().transform);
        transform.localPosition = Vector3.zero;
        transform.localScale *= scale;

        rb.bodyType = RigidbodyType2D.Kinematic;

        EntityManager.Instance?.RemoveItem(this, duration);
    }
}
