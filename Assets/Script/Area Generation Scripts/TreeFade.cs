using UnityEngine;
using System.Collections;

public class TreeFade : MonoBehaviour
{
    [SerializeField] private SpriteRenderer treeSprite;
    [SerializeField] private float fadeAlpha = 0.3f;
    [SerializeField] private float fadeSpeed = 5f;
    //[SerializeField] private float fadeRadius = 2.5f;
    [SerializeField] private float fadeZoneScale = 1.5f;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (treeSprite == null) 
            treeSprite = GetComponentInParent<SpriteRenderer>();

        PolygonCollider2D polygon = GetComponent<PolygonCollider2D>();

        if (polygon != null)
        {
            Vector2[] points = polygon.points;

            
            Vector2 center = Vector2.zero;

            foreach (Vector2 point in points)
            {
                center += point;
            }

            center /= points.Length;

            
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = center + (points[i] - center) * fadeZoneScale;
            }

            polygon.points = points;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FadeTo(fadeAlpha);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FadeTo(1f);
        }
    }

    private void FadeTo(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeCoroutine(targetAlpha));
    }

    private IEnumerator FadeCoroutine(float targetAlpha)
    {
        Color color = treeSprite.color;

        while (Mathf.Abs(color.a - targetAlpha) > 0.01f)
        {
            color.a = Mathf.MoveTowards(color.a,targetAlpha,fadeSpeed * Time.deltaTime);

            treeSprite.color = color;

            yield return null;
        }

        color.a = targetAlpha;
        treeSprite.color = color;
    }
}