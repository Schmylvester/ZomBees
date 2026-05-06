using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct IStatIcon
{
    public string name;
    public Sprite sprite;
}

public class SpriteManager : MonoBehaviour
{
    [SerializeField] Sprite[] m_towers;
    [SerializeField] Sprite[] m_enemies;
    [SerializeField] List<IStatIcon> m_icons;

    public Sprite getTowerSprite(int index) {  return m_towers[index]; }
    public Sprite getEnemySprite(int index) {return m_enemies[index]; }
    public Sprite getStatSprite(string name) { return m_icons.Find(i => i.name == name).sprite; }
}
