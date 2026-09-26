using UnityEngine;
using UnityEngine.AI;

public class AnimFaceDetector : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    public int faceDetectDirection;


    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        FaceDetect();
    }

    public int FaceDetect()
    {
        float horizontal = navMeshAgent.velocity.x;
        float vertical = navMeshAgent.velocity.y;

        // Face Up
        if (horizontal > -0.5f && horizontal < 0.5f && vertical > 0)
        {
            return faceDetectDirection = 1;
        }

        // Face Right Up
        if ((horizontal > 0.5f && vertical > 0.5f) || (horizontal < -0.5f && vertical > 0.5f))
        {
            return faceDetectDirection = 2;
        }

        // Face Right
        if ((horizontal > 0 && vertical > -0.5f && vertical < 0.5f) || (horizontal < 0 && vertical > -0.5f && vertical < 0.5f))
        {
            return faceDetectDirection = 3;
        }

        // Face Right Down
        if ((horizontal > 0.5f && vertical < -0.5f) || (horizontal < -0.5f && vertical < -0.5f))
        {
            return faceDetectDirection = 4;
        }

        // Face Down
        if (horizontal > -0.5f && horizontal < 0.5f && vertical < 0)
        {
            return faceDetectDirection = 5;
        }

        return faceDetectDirection;
    }
}
