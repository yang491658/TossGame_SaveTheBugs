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

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        //if (_collision.CompareTag("Enemy"))
        //    GameManager.Instance?.GameOver();
        //if (_collision.CompareTag("Item"))
        //    _collision.GetComponent<Item>().UseItem();
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.CompareTag("Item"))
            _collision.gameObject.GetComponent<Item>().UseItem();        
    }
}
