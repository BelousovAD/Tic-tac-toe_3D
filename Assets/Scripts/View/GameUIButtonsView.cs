using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class GameUIButtonsView : MonoBehaviour
{
    [SerializeField] private GameAreaState m_gameAreaState;

    private CanvasGroup m_canvasGroup;

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        m_gameAreaState.OnBusying += DisableInteractable;
        m_gameAreaState.OnBusied += EnableInteractable;
    }
    
    private void OnDisable()
    {
        m_gameAreaState.OnBusying -= DisableInteractable;
        m_gameAreaState.OnBusied -= EnableInteractable;
    }

    private void EnableInteractable()
    {
        m_canvasGroup.interactable = true;
    }

    private void DisableInteractable()
    {
        m_canvasGroup.interactable = false;
    }
}
