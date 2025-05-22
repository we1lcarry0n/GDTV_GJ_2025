using Unity.Cinemachine;
using UnityEngine;

public class UpgradesPlaceTracker : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerHealth _playerHealth;
    [SerializeField] private UpgradesController _upgradeController;
    [SerializeField] private LevelGeneration _levelGeneration;

    [SerializeField] private GameObject _boulderGameObject;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("UpgradePoint"))
        {
            _playerHealth.RefillHealth();
            _playerController.SetMovement(false);
            _upgradeController.InitializeUpgradeMenu();
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("BoulderStart"))
        {
            float _boulderYPosition = _levelGeneration.GetBoulderYPosition();
            // Instantiate boulder Object
            // Enable Camera Shaking
            // Enable Warning Message
        }
        if (collision.CompareTag("BoulderEnd"))
        {
            // Disable Camera Shaking
            // Disable Warning Message
        }
    }
}
