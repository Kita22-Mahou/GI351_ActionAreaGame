using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShootingStar : MonoBehaviour
{
    [Header("Position Settings (UI RectTransform)")]
    [Tooltip("จุดเริ่มต้น (นอกจอมุมขวาบน)")]
    [SerializeField] private Vector2 startPosition = new Vector2(600f, 400f);

    [Tooltip("จุดสิ้นสุด (ซ้ายล่าง)")]
    [SerializeField] private Vector2 endPosition = new Vector2(-600f, -400f);

    [Header("Timing Settings")]
    [Tooltip("ระยะเวลาที่ดาวใช้พุ่งผ่านหน้าจอ (วินาที)")]
    [SerializeField] private float fallDuration = 1.0f;

    [Tooltip("เวลาหน่วงก่อนเริ่มพุ่งดวงแรก (ใส่ให้ดาวดวงที่ 2 ตกช้ากว่า)")]
    [SerializeField] private float initialDelay = 0f;

    [Tooltip("สุ่มเวลาพักต่ำสุดก่อนพุ่งตกดวงถัดไป")]
    [SerializeField] private float minWaitTime = 2.0f;

    [Tooltip("สุ่มเวลาพักสูงสุดก่อนพุ่งตกดวงถัดไป")]
    [SerializeField] private float maxWaitTime = 4.5f;

    private RectTransform rectTransform;
    private Graphic graphicComponent; // ใช้ปรับ Alpha ความโปร่งแสงของ Image

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        graphicComponent = GetComponent<Graphic>();
    }

    private void OnEnable()
    {
        StartCoroutine(ShootingStarRoutine());
    }

    private IEnumerator ShootingStarRoutine()
    {
        // ซ่อนดาวไว้จุดเริ่มต้น
        SetAlpha(0f);
        rectTransform.anchoredPosition = startPosition;

        // รอเวลารอบแรก (ถ้าตั้งค่า Initial Delay ไว้)
        if (initialDelay > 0)
        {
            yield return new WaitForSeconds(initialDelay);
        }

        while (true)
        {
            float elapsed = 0f;

            // ช่วงดาวพุ่งตก
            while (elapsed < fallDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / fallDuration);

                // 1. เคลื่อนตำแหน่งจาก Start ไป End
                rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

                // 2. ค่อยๆ สว่างตรงกลางทาง แล้วจางหายตอนปลายทาง (Sine Wave Alpha)
                float alpha = Mathf.Sin(t * Mathf.PI);
                SetAlpha(alpha);

                yield return null;
            }

            // ซ่อนดาวเมื่อตกสุดทางแล้ว
            SetAlpha(0f);
            rectTransform.anchoredPosition = startPosition;

            // สุ่มเวลาพักก่อนดาวตกดวงถัดไป
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SetAlpha(float alpha)
    {
        if (graphicComponent != null)
        {
            Color color = graphicComponent.color;
            color.a = alpha;
            graphicComponent.color = color;
        }
    }
}