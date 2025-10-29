using System.Collections;
using UnityEngine;

public class Shield : Item
{
    #region 스케일
    private float scale = 1f;
    private float spin = 120f;
    #endregion

    #region 능력
    private Player player;

    private bool isOrigin = true;
    private int count = 3;
    private Vector3 offset;

    private bool isFired = false;
    private float duration = 5f;
    private float speed = 10f;
    #endregion

    protected override void Update()
    {
        base.Update();

        if (isActive)
            transform.Rotate(0f, 0f, spin * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (isActive && !isFired)
            transform.position = player.transform.position + offset;
    }

    public override void UseItem()
    {
        if (isActive) return;
        base.UseItem();

        transform.localScale *= scale;
        rb.bodyType = RigidbodyType2D.Kinematic;
        player = EntityManager.Instance?.GetPlayer();

        if (isOrigin)
        {
            MakeClone();
            EntityManager.Instance?.RemoveItem(this);
        }
        else StartCoroutine(FireCoroutine());
    }

    private void MakeClone()
    {
        float diag = 2f;
        float ortho = 1.2f;

        Vector3[] offs = new Vector3[]
        {
            diag * Vector3.up,
            ortho * (Vector3.up    + Vector3.right),
            ortho * (Vector3.up    + Vector3.left),
            diag * Vector3.down,
            ortho * (Vector3.down  + Vector3.right),
            ortho * (Vector3.down  + Vector3.left),
            diag * Vector3.right,
            diag * Vector3.left,
        };

        for (int i = 0; i < count; i++)
        {
            Shield clone = EntityManager.Instance.SpawnItem(data.ID, player.transform.position + offs[i])
                .GetComponent<Shield>();

            clone.SetClone();
            clone.SetOffset(offs[i]);
            clone.UseItem();
        }
    }

    private IEnumerator FireCoroutine()
    {
        yield return new WaitForSeconds(duration);
        isFired = true;
        Move(Vector2.up * speed);
    }

    #region SET
    public void SetClone() => isOrigin = false;
    public void SetOffset(Vector3 _off) => offset = _off;
    #endregion
}

