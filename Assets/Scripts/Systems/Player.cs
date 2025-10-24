using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : Entity
{
#if UNITY_EDITOR
    private void OnValidate()
    {
        data = AssetDatabase.LoadAssetAtPath<EntityData>("Assets/Scripts/ScriptableObjects/Player.asset");
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
        // TODO 테스트용 무적모드
        //if (collision.CompareTag("Enemy"))
        //GameManager.Instance?.GameOver();

        if (collision.CompareTag("Item"))
            collision.GetComponent<Item>().UseItem();
    }
}
