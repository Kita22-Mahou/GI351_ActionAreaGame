using System.Collections;
using UnityEngine;

public class OverlabDelete : MonoBehaviour
{
    [SerializeField] private Collider2D collider;
    [SerializeField] private bool isOverlab = false;

    [SerializeField] private bool selectParent;
    [SerializeField] private bool selectThis;

    private void Start()
    {
        StartCoroutine(DestroyOverlab());
    }

    IEnumerator DestroyOverlab()
    {
        yield return new WaitForSeconds(0.2f);
        if (isOverlab)
        {
            Debug.Log($"Destroy {this.name} at {transform.position}");
            if (selectParent)
            {
                GameObject parent = transform.parent.gameObject;
                Destroy(parent);
            }
            if (selectThis)
            {
                Destroy(gameObject);
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("No Count"))
            return;
        isOverlab = true;
    }
}
