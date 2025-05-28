using UnityEngine;

public class ProtectedLogic : PowerLogic
{
    [SerializeField] private ShieldHealth _shieldHealth;

    public override void ApplyUpgrade()
    {
        _shieldHealth.HalfenShieldHealth();
        _shieldHealth.MakeShieldCircle();
    }
}
