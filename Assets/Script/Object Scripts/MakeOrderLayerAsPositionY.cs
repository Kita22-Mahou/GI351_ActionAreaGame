using UnityEngine;

public class MakeOrderLayerAsPositionY : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        spriteRenderer.sortingOrder = -(int)(transform.position.y);
    }
}
