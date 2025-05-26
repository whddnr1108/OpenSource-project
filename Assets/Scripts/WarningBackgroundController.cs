using System.Collections;
using UnityEngine;

public class WarningBackgroundController : MonoBehaviour
{
    [SerializeField] private GameObject[] normalBackgrounds; // 기본 배경 2개
    [SerializeField] private GameObject warningBackground;    // 경고 배경 1개

    public void ShowWarningBackground(float duration)
    {
        StartCoroutine(WarningRoutine(duration));
    }

    private IEnumerator WarningRoutine(float duration)
    {
        // 기본 배경 비활성화
        foreach (var bg in normalBackgrounds)
        {
            if (bg != null) bg.SetActive(false);
        }

        // 워닝 배경 활성화
        if (warningBackground != null)
            warningBackground.SetActive(true);

        // 1초간 유지
        yield return new WaitForSeconds(duration);

        // 워닝 배경 비활성화
        if (warningBackground != null)
            warningBackground.SetActive(false);

        // 기본 배경 다시 활성화
        foreach (var bg in normalBackgrounds)
        {
            if (bg != null) bg.SetActive(true);
        }
    }
}
