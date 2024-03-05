using TMPro;
using UnityEngine;

public class GameStateLabelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_label;
    [SerializeField] private Referee m_referee;
    [SerializeField] private GameState m_gameState;
    [SerializeField] private GameAreaState m_gameAreaState;

    private void OnEnable()
    {
        m_referee.OnTurnChanged += SetTurnText;
        m_gameState.OnSendData += ReceiveGameStateData;
        m_gameAreaState.OnBusying += Hide;
        m_gameAreaState.OnBusied += Show;
    }

    private void OnDisable()
    {
        m_referee.OnTurnChanged -= SetTurnText;
        m_gameState.OnSendData -= ReceiveGameStateData;
        m_gameAreaState.OnBusying -= Hide;
        m_gameAreaState.OnBusied -= Show;
    }

    private void ReceiveGameStateData(GameStatus status)
    {
        switch (status)
        {
            case GameStatus.Victory:
                SetWinText();
                break;
            case GameStatus.Draw:
                SetDrawText();
                break;
            default:
                break;
        }
    }

    private void Hide()
    {
        m_label.alpha = 0f;
    }

    private void Show()
    {
        m_label.alpha = 1f;
    }

    private void SetTurnText()
    {
        m_label.text = "Turn: " + m_referee.CurrentPlayer.Name;
    }

    private void SetWinText()
    {
        m_label.text = "Winner is " + m_referee.CurrentPlayer.Name + "!";
    }

    private void SetDrawText()
    {
        m_label.text = "Draw!";
    }
}
