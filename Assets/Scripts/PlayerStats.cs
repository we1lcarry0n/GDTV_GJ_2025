using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private PlayerController _controller;
    [SerializeField] private ShieldHealth _shieldHealth;

    [SerializeField] private float _playerHealthMax;
    [SerializeField] private float _playerAcceleration;
    [SerializeField] private float _playerDeceleration;
    [SerializeField] private float _playerSpeedY;
    [SerializeField] private float _playerShieldHealth;

    public void SyncronizeUpgrades()
    {

    }
}
