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
    [SerializeField] private EnemyData[] datas;
    private readonly Dictionary<int, EnemyData> dataDic = new Dictionary<int, EnemyData>();

    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float spawnPosY = 8f;
    [SerializeField][Min(0f)] private float spawnDelay = 3f;
    private Coroutine spawnRoutine;

    [Header("Entities")]
    [SerializeField] private Transform inGame;
    [SerializeField] private Transform player;
    [SerializeField] private Transform enemies;
    [SerializeField] private List<Enemy> spawned = new List<Enemy>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (enemyBase == null)
            enemyBase = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/EnemyBase.prefab");

        if (spawnPos == null)
            spawnPos = transform.Find("SpawnPos");

        string[] guids = AssetDatabase.FindAssets("t:EnemyData", new[] { "Assets/Scripts/ScriptableObjects" });
        var list = new List<EnemyData>(guids.Length);
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            var data = AssetDatabase.LoadAssetAtPath<EnemyData>(path);
            if (data != null) list.Add(data);
        }
        datas = list.OrderBy(d => d.ID).ThenBy(d => d.Name).ToArray();
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

        dataDic.Clear();
        for (int i = 0; i < datas.Length; i++)
        {
            var d = datas[i];
            if (d != null && !dataDic.ContainsKey(d.ID))
                dataDic.Add(d.ID, d);
        }


        SetEntity();
    }

    #region 소환
    private EnemyData FindByID(int _id) => dataDic.TryGetValue(_id, out var _data) ? _data : null;

    public Enemy Spawn(int _id = 0, Vector2? _pos = null)
    {
        EnemyData data = FindByID((_id == 0) ? Random.Range(0, datas.Length) : _id);
        if (data == null) return null;

        Vector2 pos;
        if (_pos.HasValue)
        {
            pos = _pos.Value;
        }
        else
        {
            var cam = Camera.main;

            float halfWidth = cam.orthographicSize * cam.aspect;
            float minX = cam.transform.position.x - halfWidth;
            float maxX = cam.transform.position.x + halfWidth;

            float x = Random.Range(minX, maxX);
            pos = new Vector2(x, spawnPos.position.y);
        }

        Enemy e = Instantiate(enemyBase, pos, Quaternion.identity, enemies)
            .GetComponent<Enemy>();


        e.SetData(data.Clone());
        spawned.Add(e);

        return e;
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
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    #endregion

    #region 제거
    public void Despawn(Enemy _unit)
    {
        if (_unit == null) return;

        spawned.Remove(_unit);

        Destroy(_unit.gameObject);
    }

    public void DespawnAll()
    {
        for (int i = spawned.Count - 1; i >= 0; i--)
            Despawn(spawned[i]);
    }
    #endregion

    #region 동작
    #endregion

    #region SET
    public void SetEntity()
    {
        if (inGame == null) inGame = GameObject.Find("InGame")?.transform;
        if (player == null) player = GameObject.Find("InGame/Player")?.transform;
        if (enemies == null) enemies = GameObject.Find("InGame/Enemies")?.transform;

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
