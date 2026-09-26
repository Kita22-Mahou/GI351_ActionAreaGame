using UnityEngine;

public class PlayerFlipAnim : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector3 originalScale;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        originalScale = transform.localScale;
    }
    private void Update()
    {
        float horizontal = rb.linearVelocityX;

        if (horizontal > 0)
        {
            // หันขวา
            transform.localScale = new Vector3(
                Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
        else if (horizontal < 0)
        {
            // หันซ้าย
            transform.localScale = new Vector3(
                -Mathf.Abs(originalScale.x),
                originalScale.y,
                originalScale.z
            );
        }
    }
}
