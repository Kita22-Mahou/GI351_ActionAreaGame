using UnityEngine;
using UnityEngine.AI;

public class EnemyRangeFlipSprite : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private EnemyRange2 enemyRange2;

    private Vector3 originalScale;

    private void Start()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        enemyRange2 = GetComponentInParent<EnemyRange2>();

        originalScale = transform.localScale;
    }

    private void Update()
    {
        // กำลังเดิน
        if (navMeshAgent.velocity.sqrMagnitude > 0.01f)
        {
            FlipByMovement();
        }
        // ยืนนิ่ง
        else
        {
            FlipByTarget();
        }
    }

    private void FlipByMovement()
    {
        float horizontal = navMeshAgent.velocity.x;

        if (horizontal > 0)
        {
            FaceRight();
        }
        else if (horizontal < 0)
        {
            FaceLeft();
        }
    }

    private void FlipByTarget()
    {
        Transform target = enemyRange2.FindClosestTarget();

        if (target == null)
        {
            return;
        }

        float horizontal = target.position.x - transform.position.x;

        if (horizontal > 0)
        {
            FaceRight();
        }
        else if (horizontal < 0)
        {
            FaceLeft();
        }
    }

    private void FaceRight()
    {
        transform.localScale = new Vector3(
            Mathf.Abs(originalScale.x),
            originalScale.y,
            originalScale.z
        );
    }

    private void FaceLeft()
    {
        transform.localScale = new Vector3(
            -Mathf.Abs(originalScale.x),
            originalScale.y,
            originalScale.z
        );
    }
}
