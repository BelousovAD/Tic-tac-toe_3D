using UnityEngine;
using UnityEngine.Pool;

public class PooledObject : MonoBehaviour
{
    private const float k_absMaxSpawnDegree = 180f;

    private IObjectPool<PooledObject> m_objectPool;

    public IObjectPool<PooledObject> ObjectPool { set => m_objectPool = value; }

    public void Activate()
    {
        gameObject.transform.rotation = Quaternion.Euler(
                    Random.Range(-k_absMaxSpawnDegree, k_absMaxSpawnDegree),
                    Random.Range(-k_absMaxSpawnDegree, k_absMaxSpawnDegree),
                    Random.Range(-k_absMaxSpawnDegree, k_absMaxSpawnDegree)
            );
    }

    public void Deactivate()
    {
        m_objectPool.Release(this);
    }
}
