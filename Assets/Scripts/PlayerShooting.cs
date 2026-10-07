using TMPro;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Attack")]
    public float attackDistance = 5f;
    public float attackWidth = 1f;
    public float attackHeight = 3f;
    public int damage = 10;
    public int bullets;

    [Header("Layers")]
    public LayerMask enemyLayer;
    public GameObject bulletIcon;
    public float money = 0;
    public float moneyFromKill = 200;
    public TMP_Text moneyText;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && bullets > 0)
        {
            Attack();
        }
        if(bullets == 1)
        {
            bulletIcon.SetActive(true);
        }
        else
        {
            bulletIcon.SetActive(false);
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
            money += moneyFromKill;
            moneyText.text = money.ToString() + "$";
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
    public void GetBullets()
    {
        if(bullets == 0)
        {
            bullets +=1;
        }
    }
}
