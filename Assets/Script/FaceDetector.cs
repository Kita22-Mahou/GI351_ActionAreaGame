using UnityEngine;

public class FaceDetector : MonoBehaviour
{
    private Rigidbody2D rb;
    public int faceDetectDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        FaceDetect();
    }

    public int FaceDetect()
    {
        float horizontal = rb.linearVelocityX;
        float vertical = rb.linearVelocityY;

        if (horizontal == 0 && vertical > 0) // Face Up
        {
            return faceDetectDirection = 1;
        }
        if (horizontal > 0 && vertical > 0) // Face Right Up
        {
            return faceDetectDirection = 2;
        }
        if (horizontal > 0 && vertical == 0) // Face Right
        {
            return faceDetectDirection = 3;
        }
        if (horizontal > 0 && vertical < 0) // Face Right Down
        {
            return faceDetectDirection = 4;
        }
        if (horizontal == 0 && vertical < 0) // Face Down
        {
            return faceDetectDirection = 5;
        }
        if (horizontal < 0 && vertical < 0) // Face Left Down
        {
            return faceDetectDirection = 6;
        }
        if (horizontal < 0 && vertical == 0) // Face Left 
        {
            return faceDetectDirection = 7;
        }
        if (horizontal < 0 && vertical > 0) // Face Left Up
        {
            return faceDetectDirection = 8;
        }

        return faceDetectDirection;
    }
}
