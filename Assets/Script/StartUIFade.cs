using UnityEngine;
using System.Collections;

public class StartUIFade : MonoBehaviour
{
    [SerializeField] private float waitTime = 5f;
    [SerializeField] private float fadeDuration = 2f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        canvasGroup.alpha = 1f;
        StartCoroutine(FadeUI());
    }

    private IEnumerator FadeUI()
    {
        // รอ 5 วินาที
        yield return new WaitForSeconds(waitTime);

        // Fade จาก 1 -> 0 ใน 2 วินาที
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(
                1f,
                0f,
                timer / fadeDuration
            );

            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}