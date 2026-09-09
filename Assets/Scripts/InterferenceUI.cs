using UnityEngine;

public class InterferenceUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private RectTransform[] grasses;

    [SerializeField] private float swayAngle = 12f;
    [SerializeField] private float swaySpeed = 3f;
    [SerializeField] private float moveAmount = 8f;

    private bool isShowing = false;
    private float endTime;
    private Vector2[] basePos;

    private void Awake()
    {

        basePos = new Vector2[grasses.Length];
        for (int i = 0; i < grasses.Length; i++)
        {
            basePos[i] = grasses[i].anchoredPosition;
        }
    }

    private void Update()
    {
        if (!isShowing) return;

        for (int i = 0; i < grasses.Length; i++)
        {
            float offset = i * 0.7f;

            float angle = Mathf.Sin(Time.unscaledTime * swaySpeed + offset) * swayAngle;
            float x = Mathf.Sin(Time.unscaledTime * swaySpeed + offset) * moveAmount;

            grasses[i].localRotation = Quaternion.Euler(0, 0, angle);
            grasses[i].anchoredPosition = basePos[i] + new Vector2(x, 0);
        }

        if (Time.unscaledTime >= endTime)
        {
            HideInterference();
        }
    }

    public void ShowInterference(float duration)
    {
        panel.SetActive(true);
        isShowing = true;
        endTime = Time.unscaledTime + duration;
    }

    public void HideInterference()
    {
        isShowing = false;
        panel.SetActive(false);

        for (int i = 0; i < grasses.Length; i++)
        {
            grasses[i].localRotation = Quaternion.identity;
            grasses[i].anchoredPosition = basePos[i];
        }
    }
}