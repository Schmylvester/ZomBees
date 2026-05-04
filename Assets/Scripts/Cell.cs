using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Cell : MonoBehaviour
{
    [SerializeField] Color m_defaultColor;
    [SerializeField] Color m_generalColour;
    [SerializeField] Color m_blockedColour;
    [SerializeField] Color m_highlightColor;
    Vector2Int m_cellIndex;
    Tower m_tower = null;
    public Tower tower {  get { return m_tower; } }
    public Vector2Int cellIndex
    { 
        get { return m_cellIndex; }
        set
        {
                m_cellIndex = value;
                gameObject.name = value.y + "." + value.x;
        }
    }
    SpriteRenderer m_renderer;
    bool m_isBase = false;
    List<Cell> m_neighbours = new();
    bool m_accessible = true;
    public bool accessible { get  { return m_accessible; } }

    private void Awake()
    {
        m_renderer = GetComponent<SpriteRenderer>();
        setDefaultColour(m_renderer.color);
    }

    public List<Cell> getNeighbours() { return m_neighbours; }
    public void setNeighbour(Cell _cell, bool handshake) {
        m_neighbours.Add(_cell);
        if (handshake)
        {
            _cell.setNeighbour(this, false);
        }
    }

    public void setBase() {
        m_isBase = true;
        setDefaultColour(Color.cyan);
    }

    public bool getBase() { return m_isBase; }

    public void addHighlight()
    {
        m_renderer.color = Color.Lerp(m_defaultColor, m_highlightColor, m_highlightColor.a);
        if (m_tower)
        {
            m_tower.addHighlight();
        }
    }

    public void removeHighlight()
    {
        m_renderer.color = m_defaultColor;
        if (m_tower)
        {
            m_tower.removeHighlight();
        }
    }

    public bool isEdge()
    {
        return m_neighbours.Count < 6;
    }

    private void setDefaultColour(Color _defaultColor)
    {
        m_defaultColor = _defaultColor;
        m_renderer.color = m_defaultColor;
    }

    public void setAccessible(bool _accessible)
    {
        m_accessible = _accessible;
        setDefaultColour(m_accessible ? m_generalColour : m_blockedColour);
    }

    public void addTower(Tower _tower, bool _blockCell = false)
    {
        m_tower = _tower;
        _tower.transform.position = transform.position;
        if (_blockCell)
        {
            setAccessible(false);
        }
    }
}
