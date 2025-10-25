using UnityEngine;

public class FlowerBomb : Item
{
    [SerializeField] private float  duration = 30f;

    public override void UseItem()
    {
        base.UseItem();

        Stop();
        transform.localScale *= 3f;
        EntityManager.Instance?.RemoveItem(this, duration);
    }
}
