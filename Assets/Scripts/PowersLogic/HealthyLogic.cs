using UnityEngine;

public class HealthyLogic : PowerLogic
{
    [SerializeField] private PlayerHealth _playerHealth;

    public override void ApplyUpgrade()
    {
        _playerHealth.IncreaseHealthByPercentage(1.5f);
    }
}
