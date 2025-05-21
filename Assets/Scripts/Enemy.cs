using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _xRangeDetection;
    [SerializeField] private float _shootingRate;
    [SerializeField] private float _playerOffsetX;
    [SerializeField] private GameObject _projectilePrefab;

    private Transform _playerTransform;
    private float _timeElapsed;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _timeElapsed = _shootingRate;
    }

    private void Update()
    {
        if (!IsPlayerDetected())
        {
            return;
        }
        if (_timeElapsed >= _shootingRate)
        {
            Shoot();
            _timeElapsed = 0;
        }
        _timeElapsed += Time.deltaTime;
    }

    private bool IsPlayerDetected()
    {
        float enemyXPosition = transform.position.x;
        float playerXPosition = _playerTransform.position.x;
        if (Mathf.Abs(enemyXPosition - playerXPosition) < _xRangeDetection)
        {
            return true;
        }
        return false;
    }

    private void Shoot()
    {
        Debug.Log("Shoot");
        GameObject projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        Vector2 desiredVector = (_playerTransform.position + new Vector3(_playerOffsetX, 0, 0)) - transform.position;
        projectile.GetComponent<Projectile>().InitializeProjectile(desiredVector);
    }

}
