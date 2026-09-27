using UnityEngine;

public class AnimDestroy : MonoBehaviour
{
    void AnimationDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
