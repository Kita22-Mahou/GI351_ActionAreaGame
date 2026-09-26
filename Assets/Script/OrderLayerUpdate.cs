using UnityEngine;

public class OrderLayerUpdate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] sr;
    public int order;

    private void LateUpdate()
    {
        for (int i = 0; i < sr.Length; i++)
        {
            sr[i].sortingOrder = -(int)(transform.position.y);
            order = -(int)(transform.position.y);
        };
    }
}
