using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class BallSpawner : MonoBehaviour
{
    private const int k_defaultCapacity = 14, k_maxCapacity = 14;

    public static event UnityAction OnSpawning;
    public static event UnityAction<BallInfo> OnSpawned;

    [SerializeField] private PooledObject m_ballPrefab;
    private IObjectPool<PooledObject> m_objectPool;
    private int m_playerId = -1;

    public int PlayerId { get { return m_playerId; } set { m_playerId = value; } }

    private void Awake()
    {
        m_objectPool = new ObjectPool<PooledObject>(CreateBall,
            OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
            defaultCapacity: k_defaultCapacity, maxSize: k_maxCapacity);
    }

    private void OnEnable()
    {
        BallSpawnPosition.OnSendData += SpawnBall;
    }

    private void OnDisable()
    {
        BallSpawnPosition.OnSendData -= SpawnBall;
    }

    private void SpawnBall(BallSpawnPosition ballSpawnPosition)
    {
        OnSpawning?.Invoke();

        BallInfo instance = new(m_playerId, ballSpawnPosition.Position, m_objectPool.Get());
        instance.Ball.gameObject.transform.position = ballSpawnPosition.gameObject.transform.position;
        instance.Ball.Activate();

        OnSpawned?.Invoke(instance);
    }

    private PooledObject CreateBall()
    {
        PooledObject ballInstance = Instantiate(m_ballPrefab);
        ballInstance.ObjectPool = m_objectPool;
        return ballInstance;
    }

    private void OnReleaseToPool(PooledObject pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
        pooledObject.gameObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    private void OnGetFromPool(PooledObject pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    private void OnDestroyPooledObject(PooledObject pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
}
