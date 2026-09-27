using UnityEngine;
using UnityEngine.AI;

public class EnemyRangeFaceDetector : MonoBehaviour
{
    private EnemyRange2 enemyRange2;

    public int faceDetectDirection;

    private Transform target;

    private void Start()
    {
        enemyRange2 = GetComponent<EnemyRange2>();
    }

    private void Update()
    {
        target = enemyRange2.FindClosestTarget();

        if (target != null)
        {
            FaceDetect();
        }
    }

    public int FaceDetect()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle += 360f;
        }

        // Up
        if (angle >= 67.5f && angle < 112.5f)
        {
            return faceDetectDirection = 1;
        }

        // Up Right / Up Left
        if (angle >= 22.5f && angle < 67.5f ||
            angle >= 112.5f && angle < 157.5f)
        {
            return faceDetectDirection = 2;
        }

        // Right / Left
        if (angle >= 337.5f || angle < 22.5f ||
            angle >= 157.5f && angle < 202.5f)
        {
            return faceDetectDirection = 3;
        }

        // Down Right / Down Left
        if (angle >= 202.5f && angle < 247.5f ||
            angle >= 292.5f && angle < 337.5f)
        {
            return faceDetectDirection = 4;
        }

        // Down
        if (angle >= 247.5f && angle < 292.5f)
        {
            return faceDetectDirection = 5;
        }

        return faceDetectDirection;
    }
}
