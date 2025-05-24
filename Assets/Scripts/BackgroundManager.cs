using System.Collections;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [Header("배경 오브젝트")]
    public GameObject normalBackground;   // 평소 배경
    public GameObject warningBackground;  // 워닝 화면 배경

    /// <summary>
    /// 워닝 배경으로 전환하고 duration(초)만큼 유지 후 원래 배경으로 복구
    /// </summary>
    /// <param name="duration">워닝 배경 유지 시간(초)</param>
    public void ShowWarningBackground(float duration)
    {
        StartCoroutine(WarningRoutine(duration));
    }

    private IEnumerator WarningRoutine(float duration)
    {
        normalBackground.SetActive(false);
        warningBackground.SetActive(true);

        yield return new WaitForSeconds(duration);

        warningBackground.SetActive(false);
        normalBackground.SetActive(true);
    }
}
