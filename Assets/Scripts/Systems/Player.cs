using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : Entity
{
#if UNITY_EDITOR
    private void OnValidate()
    {
        data = AssetDatabase.LoadAssetAtPath<PlayerData>("Assets/Scripts//ScriptableObjects/Player.asset");
    }
#endif

    protected override void Awake()
    {
        base.Awake();

        col.isTrigger = false;

        SetData(data);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GameManager.Instance?.GameOver();
        }
    }
}
