using UnityEngine;

public class SpeedLogic : PowerLogic
{
    [SerializeField] private PlayerController _playerController;

    public override void ApplyUpgrade()
    {
        _playerController.IncreaseSpeedX(1);
    }
}
