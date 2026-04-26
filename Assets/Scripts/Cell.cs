using UnityEngine;
using System.Collections.Generic;

public class Cell : MonoBehaviour
{
    bool m_isOuter = false;
    bool m_isBase = false;
    List<Cell> m_neighbours = new();

    public List<Cell> getNeighbours() { return m_neighbours; }
    public void setNeighbour(Cell _cell, bool handshake) {
        m_neighbours.Add(_cell);
        if (handshake)
        {
            _cell.setNeighbour(this, false);
        }
    }
}
