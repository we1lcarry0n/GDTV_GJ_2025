using System.Collections;
using UnityEngine;

public class ShieldHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _shieldRestorationRate;
    [SerializeField] private float _shieldRestorationTime;
    [SerializeField] private float _shieldRestorationAfterBreak;
    [SerializeField] private Collider2D _shieldCollider;
    [SerializeField] private SpriteRenderer _shieldRenderer;

    [SerializeField] private GameObject _normalShieldMask;
    [SerializeField] private GameObject _circleShieldMask;
    [SerializeField] private Collider2D _secondaryShieldCollider;

    private float _currentHealth;
    private float _timeSinceLastDamage;
    private bool _isBroken = false;

    private void Start()
    {
        RefillHealth();
    }

    private void Update()
    {
        if (_isBroken)
        {
            return;
        }
        if (_currentHealth == _maxHealth)
        {
            return;
        }
        if (_timeSinceLastDamage < _shieldRestorationTime)
        {
            _timeSinceLastDamage += Time.deltaTime;
            return;
        }
        _currentHealth += _shieldRestorationRate * Time.deltaTime;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
    }

    public void ReceiveDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        if (_currentHealth == 0)
        {
            BreakShield();
        }
    }

    public void RefillHealth()
    {
        _currentHealth = _maxHealth;
    }

    private void BreakShield()
    {
        _isBroken = true;
        _shieldCollider.enabled = false;
        _shieldRenderer.enabled = false;
        StartCoroutine(ShieldRestorationRoutine());
    }

    private IEnumerator ShieldRestorationRoutine()
    {
        yield return new WaitForSeconds(_shieldRestorationAfterBreak);
        _currentHealth = _maxHealth * .75f;
        _shieldCollider.enabled = true;
        _shieldRenderer.enabled = true;
        _isBroken = false;
    }

    public void ModifyShieldHealth(float modifier)
    {
        _maxHealth *= modifier;
        RefillHealth();
        _shieldRenderer.color = Color.green;
    }

    public void HalfenShieldHealth()
    {
        _maxHealth *= .5f;
        RefillHealth();
    }

    public void MakeShieldCircle()
    {
        _normalShieldMask.SetActive(false);
        _circleShieldMask.SetActive(true);
        _shieldCollider.enabled = false;
        _secondaryShieldCollider.enabled = true;
        _shieldCollider = _secondaryShieldCollider;
    }

    public void RemoveShield()
    {
        _isBroken = true;
        _shieldCollider.enabled = false;
        _shieldRenderer.enabled = false;
    }
}
