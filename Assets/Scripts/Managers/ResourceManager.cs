using UnityEngine;
using UnityEngine.UI;

public struct IStatChange
{
    public int value;
    public float percentage;
}

public class ResourceManager : MonoBehaviour
{
    public delegate void ResourceEmpty();
    public ResourceEmpty onResourceEmpty;

    public delegate void ResourceChange(IStatChange _newVal);
    public ResourceChange onResourceChange;

    [SerializeField] Transform m_resourceBar;

    [SerializeField] int m_maxResouce = 100;
    int m_currentResource;

    [SerializeField] float m_showTime = 0f;
    float m_currentShowTimer = 0f;

    [SerializeField] Image[] m_display;

    private void Start()
    {
        m_currentResource = m_maxResouce;
        if (m_showTime > 0)
        {
            setVisible(false);
        }
    }

    private void Update()
    {
        if (m_resourceBar)
        {
            m_resourceBar.localScale = new Vector3((float)m_currentResource / m_maxResouce, 1, 1);
        }
        if (m_showTime > 0 && m_currentShowTimer > 0)
        {
            m_currentShowTimer -= Time.deltaTime;
            if (m_currentShowTimer < 0)
            {
                setVisible(false);
            }
        }
    }

    public void reduceResource(int reduction, bool alwaysValue = false)
    {
        if (alwaysValue)
        {
            reduction = Mathf.Max(reduction, 1);
        }
        m_currentResource -= reduction;
        updateResourceValue(-reduction);
    }

    public void addResource(int increment, bool alwaysValue = false)
    {
        if (alwaysValue) {
            increment = Mathf.Max(increment, 1);
        }
        updateResourceValue(increment);
    }

    void updateResourceValue(int _mod)
    {
        m_currentResource = Mathf.Clamp(m_currentResource + _mod, 0, m_maxResouce);
        if (m_currentResource <= 0)
        {
            m_currentResource = 0;
            onResourceEmpty?.Invoke();
        }

        if (m_showTime > 0)
        {
            m_currentShowTimer = m_showTime;
            setVisible(true);
        }

        onResourceChange?.Invoke(new() {
            value = m_currentResource,
            percentage = (float)m_currentResource / m_maxResouce
        });
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

    void setVisible(bool visible)
    {
        foreach (var display in m_display)
        {
            display.enabled = visible;
        }
    }
}
