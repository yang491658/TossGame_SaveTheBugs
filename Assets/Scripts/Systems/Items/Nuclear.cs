using UnityEngine;

public class Nuclear : Item
{
    #region ������
    [Header("Scale")]
    [SerializeField] private float scale = 1.2f;
    [SerializeField] private float spin = -120f;
    #endregion

    #region �ɷ�
    [Header("Ability")]
    private bool isOrigin = true;
    [SerializeField] private int count = 3;
    [SerializeField] private float gap = 1.5f;
    [SerializeField] private float speed = 15f;
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
            CopySelf();
            EntityManager.Instance?.RemoveItem(this);
        }
        else Fire();
    }

    private void CopySelf()
    {
        Vector3 c = new Vector3(AutoCamera.WorldRect.center.x, AutoCamera.WorldRect.yMin, 0f);

        for (int i = 0; i < count; i++)
        {
            int k = i == 0 ? 0 : ((i % 2 == 1) ? (i + 1) / 2 : -i / 2);
            Vector3 pos = new Vector3(c.x + gap * k, c.y, 0f);

            Nuclear copy = EntityManager.Instance.SpawnItem(data.ID, pos)
                .GetComponent<Nuclear>();

            copy.SetClone();
            copy.UseItem();
        }
    }

    private void Fire() => Move(Vector3.up * speed);

    #region SET
    public void SetClone() => isOrigin = false;
    #endregion
}
