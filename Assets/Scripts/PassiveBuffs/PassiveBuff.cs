using UnityEngine;

public enum EPassiveBuff
{
    // over time
    HealthOverTime,
    KillEverything,
    CloseSpawnPoints,
    MoneyOverTime,

    // one and done
    ImprovedFireRate,
    ImprovedDamage,
    SlowerEnemies,
    ArmourPiercing,

    // trigger
    HealthOnKill,
    RicochetProjectiles,
}

public abstract class PassiveBuff : MonoBehaviour {
    public abstract EPassiveBuff getBuffId();

    public abstract void overdrive();
}
