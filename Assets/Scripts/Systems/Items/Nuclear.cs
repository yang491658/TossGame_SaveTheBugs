using UnityEngine;

public class Nuclear : Item
{
    #region 스케일
    private float scale = 1f;
    private float spin = 120f;
    #endregion

    #region 능력
    private bool isOrigin = true;
    private int count = 3;
    private float speed = 10f;
    #endregion

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

        transform.localScale *= scale;

        if (isOrigin)
        {
            MakeClone();
            EntityManager.Instance?.RemoveItem(this);
        }
        else Fire();
    }

    private void MakeClone()
    {
        for (int i = 0; i < count; i++)
        {
            Nuclear clone = EntityManager.Instance.SpawnItem(data.ID)
                .GetComponent<Nuclear>();

            clone.SetClone();
            clone.UseItem();
        }
    }

    private void Fire() => Move(Vector3.up * speed);

    #region SET
    public void SetClone() => isOrigin = false;
    #endregion
}
