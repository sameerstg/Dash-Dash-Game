using System.Collections;
using UnityEngine;

public class DisappearingSprite : MonoBehaviour
{
    public float disappearTime = 2f; // Time in seconds before the sprite disappears
    public float reappearTime = 1f; // Time in seconds before the sprite reappears

    private SpriteRenderer spriteRenderer;
    private Collider2D spriteCollider;

    private void Start()
    {
        // Get the SpriteRenderer and Collider2D components attached to the sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteCollider = GetComponent<Collider2D>();

        // Start the coroutine for disappearing and reappearing
        StartCoroutine(DisappearAndReappear());
    }

    private IEnumerator DisappearAndReappear()
    {
        while (true)
        {
            // Disable the collider and sprite renderer to make the sprite disappear
            spriteCollider.enabled = false;
            spriteRenderer.enabled = false;

            // Wait for the specified disappear time
            yield return new WaitForSeconds(disappearTime);

            // Enable the collider and sprite renderer to make the sprite reappear
            spriteCollider.enabled = true;
            spriteRenderer.enabled = true;

            // Wait for the specified reappear time
            yield return new WaitForSeconds(reappearTime);
        }
    }
}
