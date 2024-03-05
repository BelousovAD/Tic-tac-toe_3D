using UnityEngine;
using UnityEngine.UI;

public class MenuBallColor : Menu<BallColor>
{
    [SerializeField] private Button m_randomColor;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_randomColor.onClick.AddListener(SelectRandomColor);
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        m_randomColor.onClick.RemoveListener(SelectRandomColor);
    }

    private void SelectRandomColor()
    {
        m_menuItems[Random.Range(0, m_menuItems.Length)].Button.onClick.Invoke();
    }
}
