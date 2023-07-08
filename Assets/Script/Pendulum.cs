using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public Transform pivot;          // The pivot point of the pendulum
    public float length = 5f;        // Length of the pendulum arm
    public float damping = 0.1f;     // Damping factor for the pendulum swing
    public float gravity = 9.8f;     // Gravitational acceleration

    private Rigidbody2D rb;
    private float angle;
    private float angularVelocity = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.useFullKinematicContacts = true;
    }

    void Update()
    {
        // Calculate the angle of the pendulum
        Vector2 dir = rb.position - (Vector2)pivot.position;
        angle = Mathf.Atan2(dir.y, dir.x);

        // Apply damping to the angular velocity
        angularVelocity *= 1f - damping * Time.deltaTime;

        // Apply gravitational force to the pendulum
        angularVelocity += gravity * Mathf.Sin(angle) * Time.deltaTime;

        // Update the angle based on the angular velocity
        angle += angularVelocity * Time.deltaTime;

        // Calculate the new position of the pendulum
        Vector2 newPos = pivot.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * length;

        // Move the pendulum to the new position
        rb.MovePosition(newPos);
    }
}
