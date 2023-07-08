using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject particleEffect;
    public float particleDuration = 2f; // Duration in seconds

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerMovement"))
        {
            // Spawn the particle effect at the position of the object
            GameObject effect = Instantiate(particleEffect, transform.position, Quaternion.identity);

            // Destroy the particle effect after the specified duration
            Destroy(effect, particleDuration);

            // Disable the object
            gameObject.SetActive(false);
        }
    }
}
