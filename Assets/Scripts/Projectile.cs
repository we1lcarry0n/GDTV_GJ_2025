using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;

    [SerializeField] private Rigidbody2D _rb2d;

    private Vector2 _desiredVector;

    private void FixedUpdate()
    {
        _rb2d.linearVelocity = _desiredVector * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shield"))
        {
            collision.GetComponent<ShieldHealth>().ReceiveDamage(_damage * 3f);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().ReceiveDamage(_damage);
            Destroy(gameObject);
        }
    }

    public void InitializeProjectile(Vector2 desiredVector)
    {
        _desiredVector = desiredVector;
    }
}
