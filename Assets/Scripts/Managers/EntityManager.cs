using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance { get; private set; }

    [Header("Data Setting")]
    [SerializeField] private GameObject enemyBase;
    [SerializeField] private GameObject itemBase;
    [SerializeField] private ItemData[] itemDatas;
    private readonly Dictionary<int, ItemData> itemDic = new Dictionary<int, ItemData>();

    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float spawnPosY = 12f;
    private Coroutine spawnRoutine;

    [SerializeField][Min(1)] private int eCount = 1;
    [SerializeField][Min(0.05f)] private float eDelay = 5f;
    [SerializeField][Min(3f)] private float iDelay = 10f;

    [Header("Entities")]
    [SerializeField] private Transform inGame;
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemyTrans;
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private Transform itemTrans;
    [SerializeField] private List<Item> items = new List<Item>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (enemyBase == null)
            enemyBase = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/EnemyBase.prefab");
        if (itemBase == null)
            itemBase = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ItemBase.prefab");

        string[] guids = AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/Scripts/ScriptableObjects" });
        var list = new List<ItemData>(guids.Length);
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            var data = AssetDatabase.LoadAssetAtPath<ItemData>(path);
            if (data != null) list.Add(data);
        }
        itemDatas = list.OrderBy(d => d.ID).ThenBy(d => d.Name).ToArray();

        if (spawnPos == null)
            spawnPos = transform.Find("SpawnPos");
    }
#endif

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        itemDic.Clear();
        for (int i = 0; i < itemDatas.Length; i++)
        {
            var d = itemDatas[i];
            if (d != null && !itemDic.ContainsKey(d.ID))
                itemDic.Add(d.ID, d);
        }

        SetEntity();
    }

    #region 적
    public Enemy SpawnEnemy(Vector2? _pos = null)
    {
        Vector2 pos = SpawnPos(_pos);

        Enemy e = Instantiate(enemyBase, pos, Quaternion.identity, enemyTrans)
            .GetComponent<Enemy>();

        enemies.Add(e);

        return e;
    }
    #endregion

    #region 아이템
    private ItemData SearchItem(int _id) => itemDic.TryGetValue(_id, out var _data) ? _data : null;

    public Item SpawnItem(int _id = 0, Vector2? _pos = null)
    {
        if (items.Count >= 5) return null;

        ItemData data = (_id == 0)
            ? itemDatas[Random.Range(0, itemDatas.Length)]
            : SearchItem(_id);
        if (data == null) return null;

        Vector2 pos = SpawnPos(_pos) + Vector2.down;

        Item i = Instantiate(itemBase, pos, Quaternion.identity, itemTrans)
            .GetComponent<Item>();

        i.SetData(data.Clone());
        items.Add(i);

        return i;
    }
    #endregion

    #region 공통
    private Vector2 SpawnPos(Vector2? _pos)
    {
        if (_pos.HasValue)
            return _pos.Value;

        var cam = Camera.main;
        float halfWidth = cam.orthographicSize * cam.aspect;
        float minX = cam.transform.position.x - halfWidth;
        float maxX = cam.transform.position.x + halfWidth;
        float x = Random.Range(minX, maxX);
        return new Vector2(x, spawnPos.position.y);
    }

    public void ToggleSpawn(bool _on)
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);

        if (_on)
            spawnRoutine = StartCoroutine(SpawnCoroutine());
        else
            spawnRoutine = null;
    }

    private IEnumerator SpawnCoroutine()
    {
        float eTimer = eDelay;
        float iTimer = iDelay;

        while (true)
        {
            eCount = Mathf.Max(1, GameManager.Instance.GetTotalScore() / 100);

            float dt = Time.deltaTime;
            eTimer += dt;
            iTimer += dt;

            eDelay = Mathf.Max(0.1f, eDelay - dt / 100f);
            iDelay = Mathf.Max(3f, iDelay - dt / 70f);

            int cnt = 0;
            while (eTimer >= eDelay && cnt++ < 4)
            {
                for (int i = 0; i < eCount; i++)
                {
                    SpawnEnemy();
                    yield return new WaitForSeconds(0.01f);
                }

                eTimer -= eDelay;
            }

            cnt = 0;
            while (iTimer >= iDelay && cnt++ < 4)
            {
                SpawnItem();
                iTimer -= iDelay;
            }


            yield return null;
        }
    }
    #endregion

    #region 제거
    public void Remove(Enemy _enemy)
    {
        if (_enemy == null) return;

        enemies.Remove(_enemy);

        Destroy(_enemy.gameObject);
    }

    public void Remove(Item _item)
    {
        if (_item == null) return;

        items.Remove(_item);

        Destroy(_item.gameObject);
    }

    public void RemoveAll()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
            Remove(enemies[i]);

        for (int i = items.Count - 1; i >= 0; i--)
            Remove(items[i]);
    }
    #endregion

    #region SET
    public void SetEntity()
    {
        if (inGame == null) inGame = GameObject.Find("InGame")?.transform;
        if (player == null) player = GameObject.Find("InGame/Player")?.transform;
        if (enemyTrans == null) enemyTrans = GameObject.Find("InGame/Enemies")?.transform;
        if (itemTrans == null) itemTrans = GameObject.Find("InGame/Items")?.transform;

        float d = AutoCamera.SizeDelta;

        Vector3 sp = spawnPos.position;
        sp.y = spawnPosY + d;
        spawnPos.position = sp;
    }
    #endregion

    #region GET
    public Player GetPlayer() => player.GetComponent<Player>();
    #endregion
}
