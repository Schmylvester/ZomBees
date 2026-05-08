using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class InfoManager : MonoBehaviour
{
    [SerializeField] Color m_defaultTextColour;
    [SerializeField] TMP_Text m_title;
    [SerializeField] TMP_Text m_description;
    UIInfo[] m_highlightableUI;
    UIInfo m_highlightedUI;
    string m_overriddenInfo = "";
    float m_overriddenTimer = 0f;

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
        if (m_overriddenInfo != "")
        {
            m_overriddenTimer -= Time.deltaTime;
            if (m_overriddenTimer < 0f)
            {
                m_overriddenInfo = "";
                m_description.color = m_defaultTextColour;
            }
            return;
        }
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

    public void overrideInfo(string _info, float _time)
    {
        overrideInfo(_info, _time, m_defaultTextColour);
    }
    public void overrideInfo(string _info, float _time, Color _colour)
    {
        m_overriddenInfo = _info;
        m_overriddenTimer = _time;
        m_description.text = _info;
        m_description.color = _colour;
        m_title.text = "";
    }
}
