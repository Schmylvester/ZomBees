using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatIcons : MonoBehaviour
{
    [SerializeField] GameObject[] m_valuedIcons;
    [SerializeField] Image[] m_standaloneIcons;
    [SerializeField] float m_numberedPositionRange;
    [SerializeField] float m_standalonePositionRange;
    TMP_Text[] m_statValues;
    Image[] m_statValueIcons;

    private void Start()
    {
        m_statValues = new TMP_Text[m_valuedIcons.Length];
        m_statValueIcons = new Image[m_valuedIcons.Length];
        for (var i = 0; i < m_valuedIcons.Length; ++i)
        {
            m_statValues[i] = m_valuedIcons[i].GetComponentInChildren<TMP_Text>();
            m_statValues[i].enabled = false;
            m_statValueIcons[i] = m_valuedIcons[i].GetComponentInChildren<Image>();
            m_statValueIcons[i].enabled = false;
        }

        foreach (var icon in m_standaloneIcons) {
            icon.enabled = false;
        }
    }

    public void setStatIcons(IStatDisplay[] _stats)
    {
        var valuedIndex = 0;
        var standaloneIndex = 0;
        if (_stats != null)
        {
            foreach (var stat in _stats) {
                var sprite = GameManager.instance.spriteManager.getStatSprite(stat.icon);
                var standaloneStat = (new string[] { "True", "False" }).Contains(stat.value);
                if (standaloneStat)
                {
                    if (stat.value == "True") {
                        m_standaloneIcons[standaloneIndex].enabled = true;
                        m_standaloneIcons[standaloneIndex].sprite = sprite;
                        standaloneIndex++;
                    }
                } else
                {
                    if (float.Parse(stat.value) != 0f)
                    {
                        m_statValueIcons[valuedIndex].enabled = true;
                        m_statValueIcons[valuedIndex].sprite = sprite;
                        m_statValues[valuedIndex].enabled = true;
                        m_statValues[valuedIndex].text = stat.value;
                        valuedIndex++;
                    }
                }
            }
        }

        for (int i = 0; i < m_valuedIcons.Length; ++i)
        {
            if (i < valuedIndex)
            {
                var x = valuedIndex <= 1 ? 0 : Mathf.Lerp(-m_numberedPositionRange, m_numberedPositionRange, (float)i / (valuedIndex - 1));
                var y = m_valuedIcons[i].transform.localPosition.y;
                m_valuedIcons[i].transform.localPosition = new Vector3(x, y);
            }
            else
            {
                m_statValues[i].enabled = false;
                m_statValueIcons[i].enabled = false;
            }
        }

        for (int i = 0; i < m_standaloneIcons.Length; ++i)
        {
            if (i < standaloneIndex)
            {
                var x = standaloneIndex <= 1 ? 0 : Mathf.Lerp(-m_standalonePositionRange, m_standalonePositionRange, (float)i / (standaloneIndex - 1));
                var y = m_standaloneIcons[i].transform.localPosition.y;
                m_standaloneIcons[i].transform.localPosition = new Vector3(x, y);
            }
            else
            {
                m_standaloneIcons[i].enabled = false;
            }
        }
    }
}
