using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] private float delay = 0;

    [Header("Destryo Chance")]
    [SerializeField] bool isDestroyChance = false;
    [SerializeField] float destroyChance = 100;
    private float num = 0;

    private void Start()
    {
        if (isDestroyChance)
        {
            num = Random.Range(0, 101);
        }

        if (num <= destroyChance)
        {
            Destroy(gameObject, delay);
        }
        else if (num >= destroyChance)
        {
            Destroy(this);
        }
    }
}
