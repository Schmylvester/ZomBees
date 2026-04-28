using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Cell : MonoBehaviour
{
    SpriteRenderer m_renderer;
    bool m_isBase = false;
    List<Cell> m_neighbours = new();
    bool m_accessible = true;
    public bool accessible { get  { return m_accessible; } }

    private void Awake()
    {
        m_renderer = GetComponent<SpriteRenderer>();
        if (Random.Range(0f, 1f) < 0.3f)
        {
            m_accessible = false;
            m_renderer.color = Color.black;
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
        m_renderer.color = Color.red;
    }

    public bool getBase() { return m_isBase; }

    public void highlight()
    {
        m_renderer.color = Color.blue;
    }
}
