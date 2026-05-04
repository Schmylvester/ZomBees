using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class InfoManager : MonoBehaviour
{
    [SerializeField] TMP_Text m_text;
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
        var infoText = "";
        List<RaycastHit2D> collisions = new();
        Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), float.MaxValue, collisions);
        var outIndex = -1;
        for (int i = 0; i < collisions.Count; i++)
        {
            var info = collisions[i].collider.GetComponent<Info>();
            if (info)
            {
                if (info.layerPriority > outIndex)
                {
                    infoText = info.getInfo();
                    outIndex = info.layerPriority;
                }
            }
        }

        if (m_highlightedUI)
        {
            infoText = m_highlightedUI.getInfo();
        }

        if (infoText != m_text.text)
        {
            m_text.text = infoText;
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
