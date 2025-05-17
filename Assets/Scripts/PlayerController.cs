using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Controls.IPlayerActions
{
    [SerializeField] private float _speedX = 2f;
    [SerializeField] private float _speedY = 2f;

    private Rigidbody2D _rb2d;
    private Controls _controls;

    private float _inputY;
    private float _yVelocityThreshold = .001f;

    public float acceleration;
    public float deceleration;

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
        //float currentSpeedY = _rb2d.linearVelocityY;
        //float desiredVelocity = Mathf.Lerp(currentSpeedY, _inputY * _speedY, interpolationTime);
        CalculateVerticalVelocity();
        //_rb2d.linearVelocityY = _inputY * _speedY;
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
                _rb2d.linearVelocityY -= deceleration;
            }
            if (_rb2d.linearVelocityY < 0)
            {
                _rb2d.linearVelocityY += deceleration;
            }
        }
        else
        {
            _rb2d.linearVelocityY += _inputY * acceleration;
            _rb2d.linearVelocityY = Mathf.Clamp(_rb2d.linearVelocityY, -_speedY, _speedY);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputY = context.ReadValue<Vector2>().y;
    }
}
