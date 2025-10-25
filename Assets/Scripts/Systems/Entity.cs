using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D), typeof(Rigidbody2D))]
public class Entity : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Collider2D col;
    protected Rigidbody2D rb;

    [SerializeField] protected EntityData data;

    protected virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        col.isTrigger = true;
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    #region 이동
    public void Move(Vector2 _velocity) => rb.linearVelocity = _velocity;
    public void Stop() => Move(Vector2.zero);
    #endregion

    #region SET
    public void SetData(EntityData _data)
    {
        data = _data;

        gameObject.name = data.Name;

        if (data.Image != null)
            sr.sprite = data.Image;
    }
    #endregion
}
