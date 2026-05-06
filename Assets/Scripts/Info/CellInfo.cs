using UnityEngine;

public class CellInfo : Info
{
    [SerializeField] Cell m_cell;
    public override IInfo getInfo()
    {
        if (m_cell.tower)
        {
            return m_cell.tower.getTowerInfo();
        }
        if (!m_cell.accessible)
        {
            return new IInfo { name = "Void", description = "There is nothing here, you can't build here and regular enemies can not traverse this." };
        }
        if (m_cell.getBase())
        {
            return new IInfo { name = "The Palace", description = "If you allow too many ants to reach here, all will be lost." };
        }
        return new IInfo { name = "Honeycomb", description = "Just a regular part of the hive, place towers here." };
    }
}
