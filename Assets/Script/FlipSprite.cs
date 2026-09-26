using UnityEngine;
using UnityEngine.AI;

public class FlipSprite : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    private Vector3 originalScale;

    private void Start()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        originalScale = transform.localScale;
    }
    private void Update()
    {
        float horizontal = navMeshAgent.velocity.x;

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
