using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class StartScene : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public float blink = 1f;

    private void Start()
    {
        if (Text != null)
        {
            StartCoroutine(BlinkText());
        }
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Co3");
        }
    }

    private System.Collections.IEnumerator BlinkText()
    {
        while (true)
        {
            for (float alpha = 1f; alpha >= 0f; alpha -= Time.deltaTime * blink)
            {
                SetTextAlpha(alpha);
                yield return null;
            }

            for (float alpha = 0f; alpha <= 1f; alpha += Time.deltaTime * blink)
            {
                SetTextAlpha(alpha);
                yield return null;
            }
        }
    }

    private void SetTextAlpha(float alpha)
    {
        if (Text != null)
        {
            Color color = Text.color;
            color.a = alpha;
            Text.color = color;
        }
    }
}