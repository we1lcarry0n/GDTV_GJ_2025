using Unity.Cinemachine;
using UnityEngine;

public class UpgradesPlaceTracker : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField] private UpgradesController _upgradeController;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>(); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("UpgradePoint"))
        {
            _playerController.SetMovement(false);
            _upgradeController.InitializeUpgradeMenu();
        }
    }
}
