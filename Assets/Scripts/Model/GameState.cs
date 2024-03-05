using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameState : MonoBehaviour
{
    public event UnityAction OnBusying;
    public event UnityAction OnBusied;
    public event UnityAction<GameStatus> OnSendData;

    [SerializeField] private MenuGameUI m_menuGameUI;

    private int m_rank = 3;
    private Cube m_cube;

    public bool IsBusy { get; private set; } = false;
    public bool IsGoing {  get; private set; } = true;

    private void OnEnable()
    {
        m_menuGameUI.OnGameStart += Initialise;
        BallSpawner.OnSpawning += UpdateGameState;
    }

    private void OnDisable()
    {
        m_menuGameUI.OnGameStart -= Initialise;
        BallSpawner.OnSpawning -= UpdateGameState;
    }

    private void Initialise()
    {
        m_cube = new(m_rank);
    }

    private void UpdateGameState()
    {
        IsBusy = true;
        OnBusying?.Invoke();

        StartCoroutine(UpdateGameStateRoutine());
    }

    private IEnumerator UpdateGameStateRoutine()
    {
        yield return StartCoroutine(m_cube.WaitGameStateRoutine());

        if (m_cube.IsGameOver == true)
        {
            if (m_cube.WinnerId == -1)
            {
                IsGoing = false;
                IsBusy = false;
                OnSendData?.Invoke(GameStatus.Draw);
            }
            else
            {
                IsGoing = false;
                IsBusy = false;
                OnSendData?.Invoke(GameStatus.Victory);
            }
        }
        else
        {
            IsGoing = true;
            IsBusy = false;
            OnSendData?.Invoke(GameStatus.Continues);
        }

        OnBusied?.Invoke();
    }
}
