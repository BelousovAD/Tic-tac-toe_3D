using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Referee : MonoBehaviour
{
    public event UnityAction OnBusying;
    public event UnityAction OnBusied;

    public event UnityAction OnTurnChanged;

    [SerializeField] private MenuGameUI m_menuGameUI;
    [SerializeField] private GameObject m_layerBallSpawnPositions;
    [SerializeField] private GameState m_gameState;

    private BallSpawnPosition[] m_ballSpawnPositions;
    private BallSpawner[] m_ballSpawners;
    private Dictionary<int, Player> m_players;
    private GameSettings m_gameSettings;
    private int m_currentTurnIndex = 0;

    public bool IsBusy { get; private set; } = false;
    public Player CurrentPlayer { get; private set; }

    private void Awake()
    {
        m_ballSpawnPositions = m_layerBallSpawnPositions.GetComponentsInChildren<BallSpawnPosition>(true);
        m_ballSpawners = GetComponentsInChildren<BallSpawner>(true);
        m_players = new Dictionary<int, Player>();
        Player newPlayer = new();
        m_players.Add(newPlayer.Id, newPlayer);
        m_gameSettings = new();

        m_ballSpawners[0].enabled = true;
        for (int i = 1; i < m_ballSpawners.Length; ++i)
        {
            m_ballSpawners[i].enabled = false;
        }
    }

    private void OnEnable()
    {
        m_menuGameUI.OnGameStart += Initialise;
        m_gameState.OnSendData += ReceiveGameStateData;
    }

    private void OnDisable()
    {
        m_menuGameUI.OnGameStart -= Initialise;
        m_gameState.OnSendData -= ReceiveGameStateData;
    }

    private void Initialise()
    {
        IsBusy = true;
        OnBusying?.Invoke();

        if (m_gameSettings.GameMode == GameMode.WithFriend)
        {
            m_ballSpawners[0].PlayerId = m_players[0].Id;
            for (int i = 1; i < m_ballSpawners.Length; ++i)
            {
                Player newPlayer = new();
                m_players.Add(newPlayer.Id, newPlayer);
                m_ballSpawners[i].PlayerId = newPlayer.Id;
            }
        }
        else if (m_gameSettings.GameMode == GameMode.WithBot)
        {
            m_ballSpawners[(int)m_gameSettings.BallColor].PlayerId = m_players[0].Id;
            foreach (BallSpawner ballSpawner in m_ballSpawners)
            {
                if (ballSpawner.PlayerId == -1)
                {
                    Bot newBot = new Bot(m_ballSpawnPositions);
                    m_players.Add(newBot.Id, newBot);
                    ballSpawner.PlayerId = newBot.Id;
                }
            }
        }

        SendTurn();
    }

    private void ReceiveGameStateData(GameStatus status)
    {
        if (status == GameStatus.Continues)
        {
            ChangeTurn();
        }
    }

    private void ChangeTurn()
    {
        IsBusy = true;
        OnBusying?.Invoke();

        m_ballSpawners[m_currentTurnIndex].enabled = false;
        m_currentTurnIndex = ++m_currentTurnIndex % m_ballSpawners.Length;
        m_ballSpawners[m_currentTurnIndex].enabled = true;

        SendTurn();
    }

    private void SendTurn()
    {
        BallSpawner currentBallSpawner = m_ballSpawners[m_currentTurnIndex];
        int currentPlayerId = currentBallSpawner.PlayerId;
        CurrentPlayer = m_players[currentPlayerId];

        IsBusy = false;
        OnTurnChanged?.Invoke();
        OnBusied?.Invoke();
    }
}
