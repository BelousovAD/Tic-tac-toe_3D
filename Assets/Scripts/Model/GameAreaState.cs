using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GameAreaRotate))]
public class GameAreaState : MonoBehaviour
{
    public event UnityAction OnBusying;
    public event UnityAction OnBusied;

    [SerializeField] private Referee m_referee;
    [SerializeField] private BallState m_ballState;
    [SerializeField] private GameState m_gameState;
    private GameAreaRotate m_gameAreaRotate;

    private bool m_isBysying = false;

    private void Awake()
    {
        m_gameAreaRotate = GetComponent<GameAreaRotate>();
    }

    private void OnEnable()
    {
        m_gameAreaRotate.OnRotating += OnBusyingHandler;
        m_referee.OnBusying += OnBusyingHandler;
        m_ballState.OnBusying += OnBusyingHandler;
        m_gameState.OnBusying += OnBusyingHandler;

        m_gameAreaRotate.OnRotated += OnBusiedHandler;
        m_referee.OnBusied += OnBusiedHandler;
        m_ballState.OnBusied += OnBusiedHandler;
        m_gameState.OnBusied += OnBusiedHandler;
    }

    private void OnDisable()
    {
        m_gameAreaRotate.OnRotating -= OnBusyingHandler;
        m_referee.OnBusying -= OnBusyingHandler;
        m_ballState.OnBusying -= OnBusyingHandler;
        m_gameState.OnBusying -= OnBusyingHandler;

        m_gameAreaRotate.OnRotated -= OnBusiedHandler;
        m_referee.OnBusied -= OnBusiedHandler;
        m_ballState.OnBusied -= OnBusiedHandler;
        m_gameState.OnBusied -= OnBusiedHandler;
    }

    private void OnBusyingHandler()
    {
        if (!m_isBysying)
        {
            m_isBysying = true;
            OnBusying?.Invoke();
        }
    }

    private void OnBusiedHandler()
    {
        if (m_isBysying)
        {
            if (!m_referee.IsBusy
                && !m_gameState.IsBusy
                && !m_ballState.IsBusy
                && !m_gameAreaRotate.IsBusy)
            {
                m_isBysying = false;
                OnBusied?.Invoke();
            }
        }
    }
}
