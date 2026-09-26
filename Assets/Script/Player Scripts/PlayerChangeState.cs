using UnityEngine;

public class PlayerChangeState : MonoBehaviour
{
    [SerializeField] private GameObject[] animations;

    private Animator[] animators;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();

        animators = new Animator[animations.Length];

        for (int i = 0; i < animations.Length; i++)
        {
            animators[i] = animations[i].GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (rb.linearVelocityX != 0 || rb.linearVelocityY != 0)
        {
            SetWalk(true);
        }
        else
        {
            SetWalk(false);
        }
    }

    private void SetWalk(bool isWalk)
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetBool("isWalk", isWalk);
        }
    }
}
