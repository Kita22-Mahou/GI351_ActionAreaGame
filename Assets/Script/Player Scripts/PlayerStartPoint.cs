using UnityEngine;

public class PlayerStartPoint : MonoBehaviour
{
    [SerializeField] private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = this.transform.position;
    }
}
