using UnityEngine;

public class OrderLayerUpdate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] sr;

    private int[] originalOrders;
    private Transform characterTransform;

    private void Start()
    {
        characterTransform = transform.parent;

        originalOrders = new int[sr.Length];

        for (int i = 0; i < sr.Length; i++)
        {
            originalOrders[i] = sr[i].sortingOrder;
        }
    }

    private void Update()
    {
        int order = Mathf.RoundToInt(-characterTransform.position.y);

        for (int i = 0; i < sr.Length; i++)
        {
            sr[i].sortingOrder = order + originalOrders[i];
        }
    }
}
