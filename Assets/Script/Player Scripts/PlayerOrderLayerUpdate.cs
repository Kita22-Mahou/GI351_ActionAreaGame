using UnityEngine;

public class PlayerOrderLayerUpdate : MonoBehaviour
{
    private SpriteRenderer sr;
    public int order;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        sr.sortingOrder = -(int)(transform.position.y);
        order = -(int)(transform.position.y);
    }
}
