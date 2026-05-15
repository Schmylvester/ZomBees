using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PanZoom : MonoBehaviour
{
    [SerializeField] Camera m_camera;
    [SerializeField] InputActionReference m_move;
    [SerializeField] InputActionReference m_scroll;

    [SerializeField] float m_maxPanFullZoom;
    [SerializeField] float m_maxPanMinZoom;
    [SerializeField] float m_panSpeed;

    [SerializeField] float m_maxZoom;
    [SerializeField] float m_minZoom;
    [SerializeField] float m_zoomSpeed;

    private void Update()
    {
        pan(m_move.action.ReadValue<Vector2>());
        zoom(m_scroll.action.ReadValue<Vector2>());
    }

    void pan(Vector2 _pan)
    {
        var offset = Time.deltaTime * m_panSpeed * _pan;
        var x = transform.localPosition.x + offset.x;
        var y = transform.localPosition.y + offset.y;

        var newPos = new Vector2(transform.localPosition.x + offset.x, transform.localPosition.y + offset.y);

        var zoomLevel = (transform.localPosition.z - m_minZoom) / (m_maxZoom - m_minZoom);
        var maxPan = Mathf.Lerp(m_maxPanFullZoom, m_maxPanMinZoom, zoomLevel);

        if (Vector2.Distance(newPos, Vector3.zero) > maxPan)
        {
            newPos = newPos.normalized * maxPan;
        }
        transform.localPosition = new Vector3(newPos.x, newPos.y, transform.localPosition.z);
    }
    
    void zoom(Vector2 _zoom)
    {
        var offset = Time.deltaTime * m_zoomSpeed * _zoom.y;
        var newPos = Mathf.Clamp(transform.localPosition.z + offset, m_minZoom, m_maxZoom);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, newPos);
    }
}
