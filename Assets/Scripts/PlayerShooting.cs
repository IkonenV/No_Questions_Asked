using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Attack")]
    public float attackDistance = 5f;
    public float attackWidth = 1f;
    public float attackHeight = 3f;
    public int damage = 10;

    [Header("Layers")]
    public LayerMask enemyLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Attack()
    {
        // BoxCastin koko
        Vector3 halfExtents = new Vector3(
            attackWidth / 2f,
            attackHeight / 2f,
            attackDistance / 2f
        );

        // BoxCastin keskikohta pelaajan etupuolella
        Vector3 boxCenter =
            transform.position +
            transform.forward * (attackDistance / 2f);

        RaycastHit[] hits = Physics.BoxCastAll(
            boxCenter,
            halfExtents,
            transform.forward,
            transform.rotation,
            0f,
            enemyLayer
        );

        foreach (RaycastHit hit in hits)
        {
            EnemyHealth enemyHealth = hit.collider.gameObject.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.Death();
            }
            else
            {
                Debug.Log("Null");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 halfExtents = new Vector3(
            attackWidth / 2f,
            attackHeight / 2f,
            attackDistance / 2f
        );

        Vector3 boxCenter =
            transform.position +
            transform.forward * (attackDistance / 2f);

        Gizmos.matrix = Matrix4x4.TRS(
            boxCenter,
            transform.rotation,
            Vector3.one
        );

        Gizmos.DrawWireCube(
            Vector3.zero,
            halfExtents * 2f
        );
    }
}
