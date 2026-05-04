using UnityEngine;

public abstract class Info : MonoBehaviour
{
    [SerializeField] int m_layerPriority;
    public int layerPriority { get {  return m_layerPriority; } }
    public abstract string getInfo();
}
