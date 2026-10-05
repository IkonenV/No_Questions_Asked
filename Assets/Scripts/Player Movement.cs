using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float jumpDistance = 2f;
    public float jumpDuration = 0.4f;
    public float jumpHeight = 1.2f;
    public float turnSpeed;
    [Header("Collision")]
public float checkDistance = 2f;
public float checkWidth = 0.8f;
public float checkHeight = 1f;
public LayerMask obstacleLayer;

    [Header("References")]
    public Transform cameraTransform;

    private bool isJumping = false;

    void Update()
    {
        if (!isJumping)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                LookAtMouse();
            }
            CheckMovementInput();
        }
    }
     void CheckMovementInput()
    {
        Vector3 input = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.W))
            input = Vector3.forward;

        else if (Input.GetKeyDown(KeyCode.S))
            input = Vector3.back;

        else if (Input.GetKeyDown(KeyCode.A))
            input = Vector3.left;

        else if (Input.GetKeyDown(KeyCode.D))
            input = Vector3.right;

        if (input == Vector3.zero)
            return;

        Vector3 direction = GetCameraDirection(input);

        if (!CanJump(direction))
        {
            return;
        }

        Vector3 targetPosition =
            transform.position + direction * jumpDistance;

        StartCoroutine(Jump(targetPosition, direction));
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

    return !blocked;
}

    Vector3 GetCameraDirection(Vector3 input)
    {
        // Kameran eteen- ja oikealle-suunta
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Poistetaan korkeussuunnan vaikutus
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Muodostetaan WASD:n mukainen suunta
        Vector3 direction =
            forward * input.z +
            right * input.x;

        direction.Normalize();

        return direction;
    }

    IEnumerator Jump(Vector3 targetPosition, Vector3 direction)
    {
        isJumping = true;

        transform.rotation = Quaternion.LookRotation(direction);

        Vector3 startPosition = transform.position;

        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime / jumpDuration;

            // Liike aloituksesta kohteeseen
            Vector3 position =
                Vector3.Lerp(startPosition, targetPosition, time);

            // Hyppykäyrä
            position.y +=
                Mathf.Sin(time * Mathf.PI) * jumpHeight;

            transform.position = position;

            yield return null;
        }

        // Varmistetaan tarkka lopullinen sijainti
        transform.position = targetPosition;

        isJumping = false;
    }
    public void LookAtMouse()
    {
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    Plane groundPlane = new Plane(Vector3.up, transform.position);

    if (groundPlane.Raycast(ray, out float distance))
    {
        Vector3 mouseWorldPosition = ray.GetPoint(distance);

        Vector3 direction =
            mouseWorldPosition - transform.position;

        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }
    }
    }
}
