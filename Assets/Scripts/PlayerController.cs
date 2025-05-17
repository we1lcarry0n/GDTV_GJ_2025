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
    private Controls _controls;

    private float _inputY;
    private float _yVelocityThreshold = .001f;

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
    }

    private void FixedUpdate()
    {
        _rb2d.linearVelocityX = _speedX;
        CalculateVerticalVelocity();
        CalculateShieldRotation();
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

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputY = context.ReadValue<Vector2>().y;
    }
}
