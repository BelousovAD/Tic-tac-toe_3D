using UnityEngine;
using UnityEngine.Events;

public class Menu<TEnum> : MonoBehaviour
{
    public static event UnityAction<TEnum> OnSendData;

    [SerializeField] protected MenuItem<TEnum>[] m_menuItems;

    protected virtual void OnEnable()
    {
        foreach (MenuItem<TEnum> item in m_menuItems)
        {
            item.Button.onClick.AddListener(() => OnSendData?.Invoke(item.Value));
        }
    }

    protected virtual void OnDisable()
    {
        foreach (MenuItem<TEnum> item in m_menuItems)
        {
            item.Button.onClick.RemoveAllListeners();
        }
    }
}
