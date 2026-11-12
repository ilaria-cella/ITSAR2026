using UnityEngine;

public class AutoTargeter : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] float eyeHeight = 1.5f;

    private void FixedUpdate()
    {
        FindNearest();
    }

    private Health FindNearest()
    {
        Vector3 eye = transform.position + Vector3.up * eyeHeight;

        // sfera: tutti i bersagli entro range
        Collider[] hits = Physics.OverlapSphere(
            transform.position, range, targetMask);

        Health currentElegibleEnemy = null;

        foreach (Collider hit in hits)
        {
            Health health = hit.GetComponentInParent<Health>();
            if (!health) continue; 
            Vector3 point = health.transform.position
                          + Vector3.up * eyeHeight;
            // linea: c'è un ostacolo tra me e lui?
            if (Physics.Linecast(eye, point, obstacleMask))
                continue;

            if(currentElegibleEnemy == null)
            {
                currentElegibleEnemy = health;
            }
            else
            {
                float currentEnemyDistance = Vector3.Distance(currentElegibleEnemy.transform.position, transform.position);
                float newEnemyDistance = Vector3.Distance(health.transform.position, transform.position);

                if (newEnemyDistance < currentEnemyDistance)
                    currentElegibleEnemy = health;
            }
            // ... tiene il più vicino
        }
        Debug.Log(currentElegibleEnemy ? currentElegibleEnemy.gameObject.name : "nothing" + "found as target");
        return currentElegibleEnemy;
    }
    // Raycast: lo usa TowerPlacer (lezione 5) per il terreno

}
