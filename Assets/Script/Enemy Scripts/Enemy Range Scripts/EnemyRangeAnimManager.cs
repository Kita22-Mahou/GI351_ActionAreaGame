using UnityEngine;
using UnityEngine.AI;

public class EnemyRangeAnimManager : MonoBehaviour
{
    [SerializeField] private GameObject[] animations;
    /*
    animations[0] = Up
    animations[1] = Up Right
    animations[2] = Right
    animations[3] = Down Right
    animations[4] = Down
    */
    private AnimFaceDetector animFaceDetector;
    private EnemyRangeFaceDetector enemyRangeFaceDetector;
    private NavMeshAgent navMeshAgent;

    private int faceDirection = 2;
    private int lastFaceDirection;

    private void Start()
    {
        animFaceDetector = GetComponentInParent<AnimFaceDetector>();
        enemyRangeFaceDetector = GetComponentInParent<EnemyRangeFaceDetector>();
        navMeshAgent = GetComponentInParent<NavMeshAgent>();

        faceDirection = animFaceDetector.faceDetectDirection;

        ChangeAnimation(animFaceDetector.faceDetectDirection);
    }

    private void Update()
    {
        // กำลังเดิน
        if (navMeshAgent.velocity.sqrMagnitude > 0.01f)
        {
            faceDirection = animFaceDetector.faceDetectDirection;
        }
        // ยืนนิ่ง
        else
        {
            faceDirection = enemyRangeFaceDetector.faceDetectDirection;
        }

        // เปลี่ยน Animation เฉพาะตอนทิศเปลี่ยน
        if (faceDirection != lastFaceDirection)
        {
            ChangeAnimation(faceDirection);
        }
    }

    private void ChangeAnimation(int direction)
    {
        for (int i = 0; i < animations.Length; i++)
        {
            animations[i].SetActive(i + 1 == direction);
        }

        lastFaceDirection = direction;
    }
}
