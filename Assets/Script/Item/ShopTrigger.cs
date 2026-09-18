using UnityEngine;
using UnityEngine.InputSystem;

public class ShopTrigger : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel;

    private bool playerInRange = false;
    private SwordMan playerScript;

    private void Start()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            ToggleShop();
        }
    }

    private void ToggleShop()
    {
        if (shopPanel == null)
            return;

        bool isOpening = !shopPanel.activeSelf;
        shopPanel.SetActive(isOpening);

        if (isOpening)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            playerScript = collision.GetComponent<SwordMan>();

            if (playerScript != null)
                playerScript.isInShop = true;

            Debug.Log("E เพื่อเปิด");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;

            if (playerScript != null)
                playerScript.isInShop = false;

            if (shopPanel != null && shopPanel.activeSelf)
            {
                shopPanel.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
    public void CloseShop()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);

        Time.timeScale = 1f;
    }

}