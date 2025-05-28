using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private TMP_Text _healthText;

    private float _currentHealth;
    private float _dmgMultiplier = 1f;

    private void Start()
    {
        RefillHealth();
    }

    public void ReceiveDamage(float amount)
    {
        _currentHealth -= amount * _dmgMultiplier;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        float dec = _currentHealth % 1;
        _healthText.text = (_currentHealth - dec).ToString();
        if (_currentHealth == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void RefillHealth()
    {
        _currentHealth = _maxHealth;
        _healthText.text = _currentHealth.ToString();
    }

    public void IncreaseHealthByPercentage(float multiplier)
    {
        _maxHealth *= multiplier;
        RefillHealth();
    }

    public void DecreaseDmgMultiplier(float multiplier)
    {
        _dmgMultiplier *= multiplier;
    }
}
