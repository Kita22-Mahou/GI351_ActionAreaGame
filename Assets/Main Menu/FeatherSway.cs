using UnityEngine;

public class FeatherSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float maxRotationAngle = 8f;

    [SerializeField] private float swaySpeed = 2.0f;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (rectTransform == null) return;

        float zAngle = Mathf.Sin(Time.time * swaySpeed) * maxRotationAngle;
        rectTransform.localRotation = Quaternion.Euler(0f, 0f, zAngle);
    }
}