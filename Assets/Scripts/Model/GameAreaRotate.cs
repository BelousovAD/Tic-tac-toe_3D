using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameAreaRotate : MonoBehaviour
{
    public event UnityAction OnStepChanged;
    public event UnityAction OnRotating;
    public event UnityAction OnRotated;

    public float CurrentStep { get; private set; }

    private readonly float[] m_steps =
    {
        45f,
        90f
    };

    [SerializeField] private MenuView m_menuGameUIView;

    [SerializeField] private float m_rotatingSpeed;
    [Range(0.005f, 1f)]
    [SerializeField] private float m_epsilonAngle;

    private float m_startAngleY;
    private int m_stepIndex = 1;
    private WaitForFixedUpdate m_wait = new WaitForFixedUpdate();
    private Coroutine m_rotateAnimationRoutine;

    public bool IsBusy {  get; private set; }

    private void Start()
    {
        m_startAngleY = gameObject.transform.eulerAngles.y;
        StartRotateAnimation();
    }

    private void OnEnable()
    {
        MenuGameUI.OnSendData += HandleCommand;
        m_menuGameUIView.OnMenuClosing += StartRotateAnimation;
        m_menuGameUIView.OnMenuOpening += StopRotateAnimation;
    }

    private void OnDisable()
    {
        MenuGameUI.OnSendData -= HandleCommand;
        m_menuGameUIView.OnMenuClosing -= StartRotateAnimation;
        m_menuGameUIView.OnMenuOpening -= StopRotateAnimation;
    }

    private void HandleCommand(GameUI command)
    {
        switch (command)
        {
            case GameUI.Step:
                ChangeStepIndex();
                break;
            case GameUI.Left:
                RotateLeft();
                break;
            case GameUI.Right:
                RotateRight();
                break;
            default:
                break;
        }
    }

    private void ChangeStepIndex()
    {
        m_stepIndex = (m_stepIndex + 1) % m_steps.Length;
        CurrentStep = m_steps[m_stepIndex];
        OnStepChanged?.Invoke();
    }

    private void RotateLeft()
    {
        OnRotating?.Invoke();
        IsBusy = true;
        StartCoroutine(RotateSmoothlyRoutine(m_steps[m_stepIndex]));
    }

    private void RotateRight()
    {
        OnRotating?.Invoke();
        IsBusy = true;
        StartCoroutine(RotateSmoothlyRoutine(-m_steps[m_stepIndex]));
    }

    private void StartRotateAnimation()
    {
        IsBusy = true;
        OnRotating?.Invoke();
        m_rotateAnimationRoutine = StartCoroutine(RotateAnimationRoutine());
    }

    private void StopRotateAnimation()
    {
        StopCoroutine(m_rotateAnimationRoutine);
        RotateToStartPosition();
    }

    private void RotateToStartPosition()
    {
        float currentAngleY  = gameObject.transform.eulerAngles.y % 90;
        float difference = m_startAngleY - currentAngleY;
        StartCoroutine(RotateSmoothlyRoutine(difference));
    }

    private IEnumerator RotateSmoothlyRoutine(float angle)
    {
        float currentAngleY = gameObject.transform.eulerAngles.y;
        float newAngleY = currentAngleY + angle;
        
        while (currentAngleY != newAngleY)
        {
            currentAngleY = Mathf.LerpAngle(currentAngleY, newAngleY, m_rotatingSpeed);
            if (Mathf.Abs(newAngleY - currentAngleY) <= m_epsilonAngle)
            {
                currentAngleY = newAngleY;
            }
            gameObject.transform.eulerAngles = new Vector3(0, currentAngleY, 0);
            yield return m_wait;
        }

        IsBusy = false;
        OnRotated?.Invoke();
    }

    private IEnumerator RotateAnimationRoutine()
    {
        while (true)
        {
            gameObject.transform.Rotate(Vector3.up, m_rotatingSpeed);
            yield return m_wait;
        }
    }
}
