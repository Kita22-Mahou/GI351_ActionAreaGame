using UnityEngine;
using System.Collections;

public class FadeUI : MonoBehaviour
{
    [SerializeField] private float fadeInDuration = 1f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        canvasGroup.alpha = 0f;

        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        float timer = 0f;

        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(
                0f,
                1f,
                timer / fadeInDuration
            );

            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}