using UnityEngine;

public class AgilePower : PowerLogic
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerController _playerController;

    public override void ApplyUpgrade()
    {
        _playerHealth.IncreaseHealthByPercentage(.75f);
        _playerController.IncreaseSpeedX(1);
        _playerController.IncreaseSpeedY(.75f);
        _playerController.ModifyAcceleration(1.2f);
        _playerController.ModifyDeceleration(1.2f);
    }
}
