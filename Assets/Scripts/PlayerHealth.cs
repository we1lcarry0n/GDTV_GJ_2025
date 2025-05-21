using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private TMP_Text _healthText;

    private float _currentHealth;

    private void Start()
    {
        RefillHealth();
    }

    public void ReceiveDamage(float amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        float dec = _currentHealth % 1;
        _healthText.text = (_currentHealth - dec).ToString();
    }

    public void RefillHealth()
    {
        _currentHealth = _maxHealth;
        _healthText.text = _currentHealth.ToString();
    }
}
