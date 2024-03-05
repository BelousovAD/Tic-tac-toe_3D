using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GameAreaState))]
public class GameAreaView : MonoBehaviour
{
    public event UnityAction OnLayerIndexChanged;

    public int CurrentLayerIndex { get; private set; }
    public int MaxLayerIndex { get; private set; }

    [SerializeField] private List<GameObject> m_layers;
    [SerializeField] private MenuGameUI m_menuGameUI;
    [SerializeField] private GameState m_gameState;
    private GameAreaState m_gameAreaState;
    private GameObject m_ballSpawnPositionsLayer;

    private void Awake()
    {
        m_gameAreaState = gameObject.GetComponent<GameAreaState>();
    }

    private void Start()
    {
        m_ballSpawnPositionsLayer = m_layers[m_layers.Count - 1];
        CurrentLayerIndex = m_layers.Count - 1;
        MaxLayerIndex = m_layers.Count - 1;
    }

    private void OnEnable()
    {
        m_menuGameUI.OnSendSliderData += ChangeLayersView;
        BallSpawner.OnSpawned += AdoptBall;
        BallSpawnPosition.OnSendData += HidePositionIfFilled;
        m_gameAreaState.OnBusying += TryHideBallSpawnPositionsLayer;
        m_gameAreaState.OnBusied += TryShowBallSpawnPositionsLayer;
    }

    private void OnDisable()
    {
        m_menuGameUI.OnSendSliderData -= ChangeLayersView;
        BallSpawner.OnSpawned -= AdoptBall;
        BallSpawnPosition.OnSendData -= HidePositionIfFilled;
        m_gameAreaState.OnBusying -= TryHideBallSpawnPositionsLayer;
        m_gameAreaState.OnBusied -= TryShowBallSpawnPositionsLayer;
    }

    private void HidePositionIfFilled(BallSpawnPosition ballSpawnPosition)
    {
        if (ballSpawnPosition.Position.z == MaxLayerIndex - 1)
        {
            ballSpawnPosition.View.Hide();
        }
    }

    private void ChangeLayersView(float layerIndex)
    {
        int newLayerindex = (int)layerIndex;
        bool islayerIndexIncreased = newLayerindex > CurrentLayerIndex;

        if (!islayerIndexIncreased)
        {
            for (int i = CurrentLayerIndex; i > newLayerindex; --i)
            {
                m_layers[i].SetActive(false);
            }
        }
        else
        {
            for (int i = CurrentLayerIndex + 1; i <= newLayerindex; ++i)
            {
                m_layers[i].SetActive(true);
            }
        }

        CurrentLayerIndex = newLayerindex;

        OnLayerIndexChanged?.Invoke();
    }

    private void AdoptBall(BallInfo ballInfo)
    {
        ballInfo.Ball.gameObject.transform.SetParent(m_layers[ballInfo.Position.z].transform);
    }

    private void TryHideBallSpawnPositionsLayer()
    {
        if (CurrentLayerIndex == m_layers.Count - 1
            && m_ballSpawnPositionsLayer.activeSelf)
        {
            m_ballSpawnPositionsLayer.SetActive(false);
        }
    }
    
    private void TryShowBallSpawnPositionsLayer()
    {
        if (CurrentLayerIndex == m_layers.Count - 1
            && !m_ballSpawnPositionsLayer.activeSelf
            && m_gameState.IsGoing)
        {
            m_ballSpawnPositionsLayer.SetActive(true);
        }
    }
}
