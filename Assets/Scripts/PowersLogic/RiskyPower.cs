using UnityEngine;

public class RiskyPower : PowerLogic
{
    [SerializeField] private ShieldHealth _shieldHealth;
    [SerializeField] private PlayerController _playerController;

    public override void ApplyUpgrade()
    {
        _shieldHealth.RemoveShield();
        _playerController.IncreaseSpeedX(2);
        _playerController.IncreaseSpeedY(1);
    }
}
