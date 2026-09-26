using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private GameObject[] animations;
    /*
    animations[0] = Up
    animations[1] = Up Right
    animations[2] = Right
    animations[3] = Down Right
    animations[4] = Down
    */
    private PlayerAnimFaceDetector animFaceDetector;
    private int faceDirection = 2;
    private int lastFaceDirection;

    private void Start()
    {
        animFaceDetector = GetComponentInParent<PlayerAnimFaceDetector>();

        faceDirection = animFaceDetector.faceDetectDirection;

        ChangeAnimation(animFaceDetector.faceDetectDirection);
    }

    void Update()
    {
        faceDirection = animFaceDetector.faceDetectDirection;

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
