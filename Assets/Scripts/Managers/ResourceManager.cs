using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public delegate void ResourceEmpty();
    public ResourceEmpty onResourceEmpty;

    [SerializeField] Transform m_resourceBar;

    [SerializeField] int m_maxResouce = 100;
    int m_currentResource;

    private void Start()
    {
        m_currentResource = m_maxResouce;
    }

    private void Update()
    {
        if (m_resourceBar)
        {
            m_resourceBar.localScale = new Vector3((float)m_currentResource / m_maxResouce, 1, 1);
        }
    }

    public void reduceResource(int reduction, bool alwaysValue = false)
    {
        if (alwaysValue)
        {
            reduction = Mathf.Max(reduction, 1);
        }
        m_currentResource -= reduction;
        if (m_currentResource <= 0)
        {
            m_currentResource = 0;
            onResourceEmpty?.Invoke();
        }
    }

    public void addResource(int increment, bool alwaysValue = false)
    {
        if (alwaysValue) {
            increment = Mathf.Max(increment, 1);
        }
        m_currentResource = Mathf.Min(m_currentResource + increment, m_maxResouce);
    }

    public void setInitResource(int _resource)
    {
        m_maxResouce = _resource;
        m_currentResource = _resource;
    }

    public bool canAfford(int cost)
    {
        return m_currentResource >= cost;
    }
}
