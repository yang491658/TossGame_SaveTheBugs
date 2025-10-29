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
    private float gap = 1.5f;
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
        Vector3 c = new Vector3(AutoCamera.WorldRect.center.x, AutoCamera.WorldRect.yMin, 0f);

        for (int i = 0; i < count; i++)
        {
            int k = i == 0 ? 0 : ((i % 2 == 1) ? (i + 1) / 2 : -i / 2);
            Vector3 pos = new Vector3(c.x + gap * k, c.y, 0f);

            Nuclear clone = EntityManager.Instance.SpawnItem(data.ID, pos)
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
