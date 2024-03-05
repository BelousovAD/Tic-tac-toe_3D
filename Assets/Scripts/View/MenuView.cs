using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class MenuView : MonoBehaviour
{
    public event UnityAction OnMenuOpening;
    public event UnityAction OnMenuOpened;
    public event UnityAction OnMenuClosing;
    public event UnityAction OnMenuClosed;

    private const float k_epsilonAlfa = 0.15f;
    private const float k_changingAlfaSpeed = 0.15f;
    private const float k_maxAlfa = 1f;
    private const float k_minAlfa = 0f;

    [SerializeField] private Button[] m_openButtons;
    [SerializeField] private Button[] m_closeButtons;

    private CanvasGroup m_canvasGroup;

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        foreach (Button openbutton in m_openButtons)
        {
            openbutton.onClick.AddListener(OpenMenu);
        }
        foreach (Button closebutton in m_closeButtons)
        {
            closebutton.onClick.AddListener(CloseMenu);
        }
    }

    private void OnDisable()
    {
        foreach (Button openbutton in m_openButtons)
        {
            openbutton.onClick.RemoveListener(OpenMenu);
        }
        foreach (Button closebutton in m_closeButtons)
        {
            closebutton.onClick.RemoveListener(CloseMenu);
        }
    }

    private void OpenMenu()
    {
        OnMenuOpening?.Invoke();
        StartCoroutine(OpenMenuRoutine());
    }

    private IEnumerator OpenMenuRoutine()
    {
        m_canvasGroup.blocksRaycasts = true;
        yield return StartCoroutine(ChangeAlfaSmoothly(k_maxAlfa));
        m_canvasGroup.interactable = true;
        OnMenuOpened?.Invoke();
    }

    private void CloseMenu()
    {
        OnMenuClosing?.Invoke();
        StartCoroutine(CloseMenuRoutine());
    }

    private IEnumerator CloseMenuRoutine()
    {
        m_canvasGroup.interactable = false;
        yield return StartCoroutine(ChangeAlfaSmoothly(k_minAlfa));
        m_canvasGroup.blocksRaycasts = false;
        OnMenuClosed?.Invoke();
    }

    private IEnumerator ChangeAlfaSmoothly(float target)
    {
        float currentAlfa = m_canvasGroup.alpha;
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while (currentAlfa != target)
        {
            currentAlfa = Mathf.Lerp(currentAlfa, target, k_changingAlfaSpeed);
            if (Mathf.Abs(target - currentAlfa) <= k_epsilonAlfa)
            {
                currentAlfa = target;
            }
            m_canvasGroup.alpha = currentAlfa;
            yield return wait;
        }
    }
}
