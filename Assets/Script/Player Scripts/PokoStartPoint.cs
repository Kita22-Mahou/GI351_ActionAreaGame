using UnityEngine;

public class PokoStartPoint : MonoBehaviour
{
    [SerializeField] private GameObject poko;

    void Start()
    {
        poko = GameObject.FindGameObjectWithTag("Car");

        poko.transform.position = this.transform.position;
    }
}
