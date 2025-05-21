using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Controls.IPlayerActions
{
    [SerializeField] private float _speedX = 2f;
    [SerializeField] private float _speedY = 2f;

    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;

    [SerializeField] private Transform _playerShield;

    private Rigidbody2D _rb2d;
    private PlayerHealth _playerHealth;
    private Controls _controls;

    private float _currentSpeedX;
    private float _inputY;
    private float _yVelocityThreshold = .001f;

    private bool _isUpgrading = false;
    private bool _isBeingHit = false;
    private Coroutine _beingHitRoutine;

    private void OnEnable()
    {
        _controls = new Controls();
        _controls.Enable();
        _controls.Player.SetCallbacks(this);
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _playerHealth = GetComponent<PlayerHealth>();
        _currentSpeedX = _speedX;
    }

    private void FixedUpdate()
    {
        if (_isUpgrading)
        {
            return;
        }
        if (!_isBeingHit)
        {
            _rb2d.linearVelocityX = _currentSpeedX;
        }
        CalculateVerticalVelocity();
        CalculateShieldRotation();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            _playerHealth.ReceiveDamage(5f);
            if (_beingHitRoutine != null)
            {
                StopCoroutine( _beingHitRoutine);
            }
            _isBeingHit = true;
            float bumpForce = collision.relativeVelocity.magnitude;
            Debug.Log(bumpForce);
            Vector2 collisionNormalVector = collision.contacts[0].normal;
            _rb2d.AddForce(collisionNormalVector * bumpForce, ForceMode2D.Impulse);
            _beingHitRoutine = StartCoroutine(RockHitRoutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dirt"))
        {
            _acceleration *= 4f;
            _deceleration /= 2f;
            _speedY *= 2f;
            _currentSpeedX *= 1.5f;
        }    
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Lava"))
        {
            _playerHealth.ReceiveDamage(.06f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Dirt"))
        {
            _acceleration /= 4f;
            _deceleration *= 2f;
            _speedY /= 2f;
            _currentSpeedX /= 1.5f;
        }
    }

    private void CalculateVerticalVelocity()
    {
        if (_inputY == 0)
        {
            if (Mathf.Abs(_rb2d.linearVelocityY) < _yVelocityThreshold)
            {
                _rb2d.linearVelocityY = 0;
                return;
            }
            if (_rb2d.linearVelocityY > 0)
            {
                _rb2d.linearVelocityY -= _deceleration;
            }
            if (_rb2d.linearVelocityY < 0)
            {
                _rb2d.linearVelocityY += _deceleration;
            }
        }
        else
        {
            _rb2d.linearVelocityY += _inputY * _acceleration;
            _rb2d.linearVelocityY = Mathf.Clamp(_rb2d.linearVelocityY, -_speedY, _speedY);
        }
    }

    private void CalculateShieldRotation()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.64f;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(_playerShield.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        _playerShield.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
    }

    private IEnumerator RockHitRoutine()
    {
        while (_currentSpeedX > 0)
        {
            _currentSpeedX -= _acceleration * 10;
            yield return new WaitForSeconds(.1f);
        }
        
        while (_currentSpeedX <= _speedX)
        {
            _currentSpeedX += _acceleration;
            _rb2d.linearVelocityX = _currentSpeedX;
            yield return new WaitForSeconds(.15f);
        }
        _isBeingHit = false;
    }

    public void SetMovement(bool isMoveAvailable)
    {
        if (isMoveAvailable)
        {
            _isUpgrading = false;
            _rb2d.linearVelocityY = 0;
            return;
        }
        if (!isMoveAvailable)
        {
            _isUpgrading = true;
            _rb2d.linearVelocityX = 0;
            _rb2d.linearVelocityY = 0;
            return;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputY = context.ReadValue<Vector2>().y;
    }
}
