using System.Collections;
using UnityEngine;

public class OverlabDelete : MonoBehaviour
{
    [SerializeField] private bool isOverlab = false;
    [SerializeField] private bool DestroyRigidbody = false;

    [SerializeField] private bool selectParent;
    [SerializeField] private bool selectThis;

    private void Start()
    {
        StartCoroutine(DestroyOverlab());
    }

    IEnumerator DestroyOverlab()
    {
        yield return new WaitForSeconds(0.2f);

        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rb) && DestroyRigidbody)
        {
            Destroy(rb);
        }

        if (!isOverlab)
            yield break;

        //Debug.Log($"Destroy {this.name} at {transform.position}");
        if (selectParent && transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else if (selectThis)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Overlab"))
        {
            isOverlab = true;
        }
    }
}
