using UnityEngine;

public class EnemyAnimatorHelper : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    public void SpawnProjectile()
    {
        _enemy.SpawnProjectile();
    }
}
