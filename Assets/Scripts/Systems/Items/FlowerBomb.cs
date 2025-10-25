using UnityEngine;

public class FlowerBomb : Item
{
    private float spin = 15f;
    private float duration = 8f;

    protected override void Update()
    {
        base.Update();

        if (isActive) transform.Rotate(0f, 0f, spin * Time.deltaTime);
    }

    public override void UseItem()
    {
        if (isActive) return;

        base.UseItem();

        Stop();
        transform.localScale *= 3.5f;

        EntityManager.Instance?.RemoveItem(this, duration);
    }
}
