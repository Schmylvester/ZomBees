using UnityEngine;

public class CellInfo : Info
{
    [SerializeField] Cell m_cell;
    public override string getInfo()
    {
        if (m_cell.tower)
        {
            return "There is a tower here";
        }
        if (!m_cell.accessible)
        {
            return "An untraversible void";
        }
        if (m_cell.getBase())
        {
            return "Your queen's palace, don't let the zombies get here.";
        }
        return "Empty space, perfect for a tower";
    }
}
