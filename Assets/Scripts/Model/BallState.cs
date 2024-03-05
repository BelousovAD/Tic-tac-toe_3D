using UnityEngine;
using UnityEngine.Events;

public class BallState : MonoBehaviour
{
    public event UnityAction OnBusying;
    public event UnityAction OnBusied;

    public bool IsBusy { get; private set; } = false;

    private void OnEnable()
    {
        BallSpawner.OnSpawning += ToggleBallState;
        Ball.OnBallSlept += ToggleBallState;
    }

    private void OnDisable()
    {
        BallSpawner.OnSpawning -= ToggleBallState;
        Ball.OnBallSlept -= ToggleBallState;
    }

    private void ToggleBallState()
    {
        if (!IsBusy)
        {
            IsBusy = true;
            OnBusying?.Invoke();
        }
        else
        {
            IsBusy = false;
            OnBusied?.Invoke();
        }
    }
}
