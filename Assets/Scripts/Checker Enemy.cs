using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CheckerEnemy : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Detection")]
    public float detectionRange = 15f;

    [Header("Jump")]
    public float jumpDistance = 2f;
    public float jumpDuration = 0.4f;
    public float jumpHeight = 1.2f;
    public float jumpCooldown = 1f;

    [Header("Collision")]
    public float checkWidth = 0.8f;
    public float checkHeight = 1f;
    public float checkDistance = 2f;
    public LayerMask obstacleLayer;

    [Header("Attack")]
    public int damage = 10;
    public float hitDistance = 1f;

    [Header("Hit Reaction")]
    public float knockbackDistance = 0.5f;
    public float knockbackDuration = 0.15f;
    public float stunDuration = 0.5f;

    [Header("References")]
    public NavMeshAgent agent;

    private bool isJumping = false;
    private bool isStunned = false;

    private float cooldown = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        // NavMeshAgent etsii reitin,
        // mutta ei liikuta hahmoa itse.
        agent.updatePosition = false;
        agent.updateRotation = false;

        agent.nextPosition = transform.position;
    }

    void Update()
    {
        if (player == null)
            return;

        if (isJumping || isStunned)
            return;

        cooldown -= Time.deltaTime;

        float distanceToPlayer =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distanceToPlayer > detectionRange)
            return;

        // Päivitetään reitti pelaajaan.
        agent.SetDestination(player.position);

        if (cooldown > 0f)
            return;

        TryJump();
    }

    void TryJump()
    {
        Vector3 direction;

        if (agent.hasPath)
        {
            direction =
                agent.steeringTarget - transform.position;
        }
        else
        {
            direction =
                player.position - transform.position;
        }

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        direction.Normalize();

        transform.rotation =
            Quaternion.LookRotation(direction);

        if (!CanJump(direction))
        {
            cooldown = 0.2f;
            return;
        }

        Vector3 targetPosition =
            transform.position +
            direction * jumpDistance;

        StartCoroutine(
            Jump(targetPosition, direction)
        );
    }

    bool CanJump(Vector3 direction)
    {
        Vector3 halfExtents = new Vector3(
            checkWidth / 2f,
            checkHeight / 2f,
            checkWidth / 2f
        );

        Vector3 boxCenter =
            transform.position +
            Vector3.up * (checkHeight / 2f);

        bool blocked = Physics.BoxCast(
            boxCenter,
            halfExtents,
            direction,
            out RaycastHit hit,
            transform.rotation,
            checkDistance,
            obstacleLayer
        );

        if (blocked)
        {
            return false;
        }

        return true;
    }

    IEnumerator Jump(
        Vector3 targetPosition,
        Vector3 direction
    )
    {
        isJumping = true;

        Vector3 startPosition =
            transform.position;

        float time = 0f;

        while (time < 1f)
        {
            time +=
                Time.deltaTime /
                jumpDuration;

            Vector3 position =
                Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    time
                );

            position.y +=
                Mathf.Sin(time * Mathf.PI) *
                jumpHeight;

            transform.position = position;

            agent.nextPosition =
                transform.position;

            yield return null;
        }

        transform.position =
            targetPosition;

        agent.nextPosition =
            transform.position;

        isJumping = false;

        // Tarkistetaan osuiko tammi pelaajaan.
        CheckPlayerHit();

        cooldown = jumpCooldown;
    }

    void CheckPlayerHit()
    {
        Vector3 difference =
            player.position -
            transform.position;

        difference.y = 0f;

        float distance =
            difference.magnitude;

        if (distance <= hitDistance)
        {
            PlayerHealth health =
                player.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }

            // Tammi lennähtää taaksepäin.
            StartCoroutine(
                HitReaction()
            );
        }
    }

    IEnumerator HitReaction()
    {
        isStunned = true;

        // Suunta pelaajasta poispäin.
        Vector3 knockbackDirection =
            transform.position -
            player.position;

        knockbackDirection.y = 0f;

        if (knockbackDirection.sqrMagnitude < 0.01f)
        {
            knockbackDirection =
                -transform.forward;
        }

        knockbackDirection.Normalize();

        Vector3 startPosition =
            transform.position;

        Vector3 targetPosition =
            startPosition +
            knockbackDirection *
            knockbackDistance;

        float time = 0f;

        // Lennähdys taaksepäin.
        while (time < 1f)
        {
            time +=
                Time.deltaTime /
                knockbackDuration;

            float smoothTime =
                Mathf.SmoothStep(0f, 1f, time);

            Vector3 position =
                Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    smoothTime
                );

            transform.position = position;

            agent.nextPosition =
                transform.position;

            yield return null;
        }

        transform.position =
            targetPosition;

        agent.nextPosition =
            transform.position;

        // Pysähtyy hetkeksi.
        yield return new WaitForSeconds(
            stunDuration
        );

        isStunned = false;

        cooldown = jumpCooldown;
    }

    void OnDrawGizmosSelected()
    {
        // Detection range
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        // BoxCast
        Gizmos.color = Color.red;

        Vector3 halfExtents = new Vector3(
            checkWidth / 2f,
            checkHeight / 2f,
            checkWidth / 2f
        );

        Vector3 center =
            transform.position +
            transform.forward *
            (checkDistance / 2f) +
            Vector3.up *
            (checkHeight / 2f);

        Gizmos.matrix =
            Matrix4x4.TRS(
                center,
                transform.rotation,
                Vector3.one
            );

        Gizmos.DrawWireCube(
            Vector3.zero,
            halfExtents * 2f +
            Vector3.forward * checkDistance
        );

        Gizmos.matrix =
            Matrix4x4.identity;
    }
}
