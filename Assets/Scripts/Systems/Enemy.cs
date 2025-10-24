using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy : Entity
{
   private float speed = 1f;

#if UNITY_EDITOR
    private void OnValidate()
    {
        data = AssetDatabase.LoadAssetAtPath<EntityData>("Assets/Scripts/ScriptableObjects/Enemy.asset");
    }
#endif

    protected override void Start()
    {
        base.Start();

        var p = EntityManager.Instance?.GetPlayer();

        Vector2 dir = (p.transform.position - transform.position).normalized;
        Move(dir * speed);
    }

    private void OnBecameInvisible()
    {
        EntityManager.Instance?.Remove(this);
    }
}
