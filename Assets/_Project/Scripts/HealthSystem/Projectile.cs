using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float damage = 5f;

    [SerializeField] float speed = 10f;
    [SerializeField] LayerMask playerMask;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Health _health))
        {
            _health.TakeDamage(damage);
        }

        Destroy(this.gameObject);
    }
}
