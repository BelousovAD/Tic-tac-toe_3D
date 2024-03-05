using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    public static event UnityAction OnBallSlept;

    private Rigidbody m_rigidbody;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (m_rigidbody.IsSleeping())
        {
            OnBallSlept?.Invoke();
            this.enabled = false;
        }
    }
}
