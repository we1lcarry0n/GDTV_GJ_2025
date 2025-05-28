using UnityEngine;

public class StrongerModifier : PowerLogic
{
    [SerializeField] private ShieldHealth _shieldHealth;

    public override void ApplyUpgrade()
    {
        _shieldHealth.ModifyShieldHealth(1.5f);
    }
}
