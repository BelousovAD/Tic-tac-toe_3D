using System;
using UnityEngine;

public class CameraFocus : MonoBehaviour
{
    [SerializeField] private Vector2 m_referenceAspect = new Vector2(9, 16);

    private Vector3 m_defaultPosition = new Vector3( 0f, Mathf.Tan(Mathf.PI / 4.5f) * 8f + 1.44f, -8f );
    private Vector3 m_defaultRotation = new Vector3(40, 0, 0);

    private void OnEnable()
    {
        SetFocus();
        Canvas.preWillRenderCanvases += SetFocus;
    }

    private void OnDisable()
    {
        Canvas.preWillRenderCanvases -= SetFocus;
    }

    private void SetFocus()
    {
        float scaleFactor = GetScaleFactor();
        gameObject.transform.position = new Vector3(
            m_defaultPosition.x,
            m_defaultPosition.y * scaleFactor,
            m_defaultPosition.z * scaleFactor
            );
        gameObject.transform.eulerAngles = m_defaultRotation;
    }

    private float GetScaleFactor()
    {
        Vector2 screenSize = new Vector2(Display.main.renderingWidth, Display.main.renderingHeight);

        float scaleFactor = 1f / MathF.Min(1f, screenSize.x / screenSize.y * m_referenceAspect.y / m_referenceAspect.x);
        return scaleFactor;
    }
}
