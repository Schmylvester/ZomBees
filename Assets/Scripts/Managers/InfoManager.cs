using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class InfoManager : MonoBehaviour
{
    [SerializeField] TMP_Text m_title;
    [SerializeField] TMP_Text m_description;
    UIInfo[] m_highlightableUI;
    UIInfo m_highlightedUI;

    private void Awake()
    {
        m_highlightableUI = FindObjectsByType<UIInfo>(FindObjectsSortMode.None);
        foreach (var ui in m_highlightableUI)
        {
            ui.onPointerEnter += enterUI;
            ui.onPointerExit += exitUI;
        }
    }

    private void OnDestroy()
    {
        foreach (var ui in m_highlightableUI)
        {
            ui.onPointerEnter -= enterUI;
            ui.onPointerExit -= exitUI;
        }
    }

    void Update()
    {
        var infoText = new IInfo();
        List<RaycastHit2D> collisions = new();
        var collision = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (collision)
        {
            var info = collision.collider.GetComponent<Info>();
            if (info)
            {
                infoText = info.getInfo();
            }
        }

        if (m_highlightedUI)
        {
            infoText = m_highlightedUI.getInfo();
        }

        if (infoText.name != m_title.text)
        {
            m_title.text = infoText.name;
            m_description.text = infoText.description;
        }
    }

    void enterUI (UIInfo _info)
    {
        m_highlightedUI = _info;
    }

    void exitUI ()
    {
        m_highlightedUI = null;
    }
}
