using TMPro;
using UnityEngine;

public class LayersViewLabel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_label;
    [SerializeField] private GameAreaView m_gameAreaView;

    private void OnEnable()
    {
        m_gameAreaView.OnLayerIndexChanged += SetLabel;
    }

    private void OnDisable()
    {
        m_gameAreaView.OnLayerIndexChanged -= SetLabel;
    }

    private void SetLabel()
    {
        int layerIndex = m_gameAreaView.CurrentLayerIndex;
        m_label.text = layerIndex == m_gameAreaView.MaxLayerIndex ? "All layers" : "Layer " + layerIndex;
    }
}
