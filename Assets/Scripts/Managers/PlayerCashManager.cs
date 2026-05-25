using TMPro;
using UnityEngine;

public class PlayerCashManager : MonoBehaviour
{
    [SerializeField] TMP_Text m_cashDisplay;
    [SerializeField] int m_startingCash;
    int m_cap = 100;
    int m_cash = 0;

    private void Start()
    {
        updateCash(m_startingCash);
    }

    public void updateCash(int _cash)
    {
        m_cash = Mathf.Clamp(m_cash + _cash, 0, m_cap);
        m_cashDisplay.text = $"${m_cash}";
    }

    public bool canAfford(int _cost)
    {
        return m_cash >= _cost;
    }
}
