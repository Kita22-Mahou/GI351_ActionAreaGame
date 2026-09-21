using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] private float delay = 0;

    private void Start()
    {
        Destroy(gameObject, delay);
    }
}
