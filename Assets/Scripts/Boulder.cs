using UnityEngine;
using UnityEngine.SceneManagement;

public class Boulder : MonoBehaviour
{
    [SerializeField] private float speedX;
    [SerializeField] private Rigidbody2D _rb2d;

    private void OnEnable()
    {
        //TriggerCameraShaking - transfer this top Player smth like Boulder Detector
    }

    private void FixedUpdate()
    {
        _rb2d.linearVelocityX = speedX;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Make proper DeathScreen
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (collision.CompareTag("BoulderEnd"))
        {
            //Make proper destructionAnimation
            Destroy(gameObject);
        }
    }
}
