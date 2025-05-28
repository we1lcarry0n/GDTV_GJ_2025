using UnityEngine;

public class ResistantLogic : PowerLogic
{
    [SerializeField] private PlayerHealth _playerHealth;
    public override void ApplyUpgrade()
    {
        _playerHealth.DecreaseDmgMultiplier(.8f);
    }
}
