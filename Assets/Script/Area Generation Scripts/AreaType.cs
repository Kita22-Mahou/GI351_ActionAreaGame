using UnityEngine;

public class AreaType : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    public int type;

    private void Start()
    {

    }

    public void AreaDestruction()
    {
        Destroy(gameObject);
    }
}
