using UnityEngine;

public struct IStatDisplay
{
    public string icon;
    public string value;
}

public struct IInfo
{
    public string name;
    public string description;
    public IStatDisplay[] stats;
}

public abstract class Info : MonoBehaviour
{
    public abstract IInfo getInfo();
}
