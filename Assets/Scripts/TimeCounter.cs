using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ECountDirection
{
    Up, Down
}

public class TimeCounter : MonoBehaviour
{
    public delegate void TimeUp();
    public TimeUp timeUp;

    [SerializeField] ECountDirection m_direction;
    [SerializeField] float m_totalTime;
    [SerializeField] bool m_active = false;

    [SerializeField] Image m_scaleCounter;
    [SerializeField] bool m_horizontalScaleCounter;
    [SerializeField] TMP_Text m_textCounter;

    [SerializeField] bool m_useMs;
    [SerializeField] bool m_useMins;

    [SerializeField] Color m_warningColour;
    [SerializeField] float m_warningLevel;
    bool m_inWarningRange = false;

    float m_count;

    public void init(float _time, ECountDirection _direction)
    {
        m_direction = _direction;
        m_totalTime = _time;
        m_active = true;
        m_count = 0;
    }

    void Update()
    {
        if (!m_active)
        {
            return;
        }

        m_count += Time.deltaTime;

        if (m_count >= m_totalTime)
        {
            m_count = m_totalTime;
            m_active = false;
            timeUp?.Invoke();
        }

        if (m_textCounter)
        {
            m_textCounter.text = getTimeAsText();
        }

        if (m_scaleCounter)
        {
            var x = m_horizontalScaleCounter ? getTimeAsScale() : 1;
            var y = m_horizontalScaleCounter ? 1 : getTimeAsScale();
            m_scaleCounter.transform.localScale = new Vector3(x, y, 1);
        }

        checkWarningRange();
    }

    void checkWarningRange()
    {
        if (m_inWarningRange)
        {
            return;
        }
        if (m_totalTime - m_count < m_warningLevel)
        {
            m_inWarningRange = true;
            if (m_scaleCounter)
            {
                m_scaleCounter.color = m_warningColour;
            }
            if (m_textCounter)
            {
                m_textCounter.color = m_warningColour;
            }
        }
    }

    public string getTimeAsText()
    {
        var timeVal = m_direction == ECountDirection.Up ? m_count : m_totalTime - m_count;
        var format = m_useMs ? "N2" : "N0";
        if (m_useMins)
        {
            var mins = (int) timeVal / 60;
            var secs = timeVal % 60;
            var secString = secs.ToString(format);
            if (secString.Length == 1)
            {
                secString = $"0{secString}";
            }
            return $"{mins}:{secString}";
        }
        return timeVal.ToString(format);
    }

    public float getTimeAsScale()
    {
        var delta = m_count / m_totalTime;
        if (m_direction == ECountDirection.Up)
        {
            return delta;
        }
        return 1 - delta;
    }
}
