using UnityEngine;

public class boomShroomUnlocked : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Poko poko = collision.GetComponent<Poko>();

        if (poko == null)
            return;

        poko.UnlockBoomShroom();

        Destroy(gameObject);
    }
}
