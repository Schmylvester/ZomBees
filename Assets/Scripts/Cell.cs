using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
public class Cell : MonoBehaviour
{
    SpriteRenderer m_renderer;
    bool m_isBase = false;
    List<Cell> m_neighbours = new();
    Color m_defaultColour;
    static Cell m_highlightedCell;

    private void Awake()
    {
        m_renderer = GetComponent<SpriteRenderer>();
        m_defaultColour = m_renderer.color;
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
        m_defaultColour = m_renderer.color;
    }

    private void Update()
    {
        m_renderer.color = (m_highlightedCell == this || m_neighbours.Find(n => n == m_highlightedCell)) ? Color.Lerp(m_defaultColour, Color.white, 0.6f) : m_defaultColour;
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (hit.collider != null)
        {
            if (hit.collider.gameObject == gameObject) {
                m_highlightedCell = this;
            }
        }
    }
}
