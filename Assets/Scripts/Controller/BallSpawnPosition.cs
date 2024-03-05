using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BallSpawnPositionView))]
public class BallSpawnPosition : MonoBehaviour
{
    public static event UnityAction<BallSpawnPosition> OnSendData;

    [SerializeField] private Vector3Int m_position = new( 0, 0, -1 );

    public Vector3Int Position { get { return m_position; } }
    public BallSpawnPositionView View { get; private set; }

    private void Awake()
    {
        View = GetComponent<BallSpawnPositionView>();
    }

    private void OnMouseUpAsButton()
    {
        if (!isActiveAndEnabled || EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        ++m_position.z;
        OnSendData?.Invoke(this);
    }
}
