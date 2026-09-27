using UnityEngine;
using UnityEngine.AI;

public class EnemyChangeState : MonoBehaviour
{
    [SerializeField] private GameObject[] animations;

    private Animator[] animators;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();

        animators = new Animator[animations.Length];

        for (int i = 0; i < animations.Length; i++)
        {
            animators[i] = animations[i].GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (navMeshAgent.velocity.x != 0 || navMeshAgent.velocity.y != 0)
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
