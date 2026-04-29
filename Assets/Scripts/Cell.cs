using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Cell : MonoBehaviour
{
    [Range(0f,1f)][SerializeField] float m_blockedCellRate;
    Vector2Int m_cellIndex;
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
    Color m_defaultColor;

    private void Awake()
    {
        m_renderer = GetComponent<SpriteRenderer>();
        setDefaultColour(m_renderer.color);
        if (Random.Range(0f, 1f) < m_blockedCellRate)
        {
            m_accessible = false;
            setDefaultColour(Color.black);
        }
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

    public void addHighlight(Color _colour, float _strength)
    {
        m_renderer.color = Color.Lerp(m_defaultColor, _colour, _strength);
    }

    public void removeHighlight()
    {
        m_renderer.color = m_defaultColor;
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
}
