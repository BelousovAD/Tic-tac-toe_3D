using System;
using TMPro;
using UnityEngine;

public class RotationStepView : MonoBehaviour
{
    private const char k_degreeSymbol = '°';

    [SerializeField] private TextMeshProUGUI m_label;
    [SerializeField] private GameAreaRotate m_gameAreaRotate;

    private void OnEnable()
    {
        m_gameAreaRotate.OnStepChanged += SetStep;
    }

    private void OnDisable()
    {
        m_gameAreaRotate.OnStepChanged -= SetStep;
    }

    private void SetStep()
    {
        m_label.text = m_gameAreaRotate.CurrentStep.ToString() + k_degreeSymbol;
    }
}
