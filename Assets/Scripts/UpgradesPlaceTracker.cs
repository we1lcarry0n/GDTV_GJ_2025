using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradesPlaceTracker : MonoBehaviour
{
    private PlayerController _playerController;
    private PlayerHealth _playerHealth;
    [SerializeField] private UpgradesController _upgradeController;
    [SerializeField] private LevelGeneration _levelGeneration;

    [SerializeField] private GameObject _boulderGameObject;
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    private bool _isShaking;

    [SerializeField] private GameObject _gameOverScreen;

    [SerializeField] private AudioSource _upgradeAS;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GameOver"))
        {
            _gameOverScreen.SetActive(true);
            StartCoroutine(GameEndRoutine());
        }
        if (collision.CompareTag("UpgradePoint"))
        {
            _upgradeAS.Play();
            _playerHealth.RefillHealth();
            _playerController.SetMovement(false);
            _upgradeController.InitializeUpgradeMenu();
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("BoulderStart"))
        {
            float _boulderYPosition = _levelGeneration.GetBoulderYPosition();
            Instantiate(_boulderGameObject, collision.gameObject.transform.position + new Vector3(-8, 6, 0), Quaternion.identity);
            _isShaking = true;
            StartCoroutine(CameraShakeRoutine());
            // Enable Warning Message
        }
        if (collision.CompareTag("BoulderEnd"))
        {
            _isShaking = false;
            StopCoroutine(CameraShakeRoutine());
            // Disable Warning Message
        }
    }

    private IEnumerator GameEndRoutine()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }

    private IEnumerator CameraShakeRoutine()
    {
        while (_isShaking)
        {
            yield return new WaitForSeconds(.5f);
            _impulseSource.GenerateImpulse();
        }
    }
}
