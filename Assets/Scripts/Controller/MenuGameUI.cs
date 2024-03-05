using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(MenuView))]
public class MenuGameUI : Menu<GameUI>
{
    public event UnityAction<float> OnSendSliderData;
    public event UnityAction OnGameStart;

    [SerializeField] private Slider m_layersViewSlider;
    private MenuView m_menuView;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_layersViewSlider.onValueChanged.AddListener(SendSliderData);
        m_menuView = GetComponent<MenuView>();
        m_menuView.OnMenuOpening += StartGame;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        m_layersViewSlider.onValueChanged.RemoveListener(SendSliderData);
        m_menuView.OnMenuOpening -= StartGame;
    }

    private void SendSliderData(float value)
    {
        OnSendSliderData?.Invoke(value);
    }

    private void StartGame()
    {
        OnGameStart?.Invoke();
    }
}
