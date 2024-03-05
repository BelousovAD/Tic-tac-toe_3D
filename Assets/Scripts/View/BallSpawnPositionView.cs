using UnityEngine;
using UnityEngine.EventSystems;

public class BallSpawnPositionView : MonoBehaviour
{
    private const string k_emission = "_EMISSION";

    private Material m_material;

    private void Awake()
    {
        m_material = GetComponent<Renderer>().material;
        if (m_material)
        {
            m_material.DisableKeyword(k_emission);
        }
        else
        {
            Debug.Log($"{gameObject.name}: material wasn't found");
        }
    }
    
    private void OnDisable()
    {
        m_material.DisableKeyword(k_emission);
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        m_material.EnableKeyword(k_emission);
    }

    private void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        m_material.DisableKeyword(k_emission);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
