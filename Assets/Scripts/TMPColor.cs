using System.Collections;
using UnityEngine;
using TMPro;

public class TMPColor : MonoBehaviour
{
    [SerializeField] private float lerpTime = 0.1f;
    [SerializeField] private GameObject cutSceneBackground;

    private TextMeshProUGUI textBossWarning;

    private void Awake()
    {
        textBossWarning = GetComponent<TextMeshProUGUI>();
    }

    private void Start()  // <- Start에서 실행
    {
        StartCoroutine(ShowCutSceneBackground());
        StartCoroutine(ColorLerpLoop());
    }

    private IEnumerator ShowCutSceneBackground()
    {
        if (cutSceneBackground != null)
        {
            cutSceneBackground.SetActive(true);
            yield return new WaitForSeconds(3.0f);  // 원하는 시간만큼
            cutSceneBackground.SetActive(false);
        }
    }

    private IEnumerator ColorLerpLoop()
    {
        while (true)
        {
            yield return StartCoroutine(ColorLerp(Color.white, Color.red));
            yield return StartCoroutine(ColorLerp(Color.red, Color.white));
        }
    }

    private IEnumerator ColorLerp(Color startColor, Color endColor)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / lerpTime;
            textBossWarning.color = Color.Lerp(startColor, endColor, percent);
            yield return null;
        }
    }
}