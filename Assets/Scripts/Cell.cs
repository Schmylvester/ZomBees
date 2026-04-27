using UnityEngine;
using System.Collections.Generic;

public class Cell : MonoBehaviour
{
    Renderer m_renderer;
    bool m_isBase = false;
    List<Cell> m_neighbours = new();

    private void Awake()
    {
        m_renderer = GetComponent<Renderer>();
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
        m_renderer.materials[0].color = Color.red;
    }
}
