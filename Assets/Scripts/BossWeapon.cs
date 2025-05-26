using System.Collections;
using UnityEngine;

public enum AttackType { CircleFire = 0, SingleFireToCenterPosition }

public class BossWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject[] projectilePrefabs; // ✅ 다양한 발사체 배열로 변경

    [SerializeField]
    private Transform[] FirePoint; // 손가락 위치

    public void StartFiring(AttackType attackType)
    {
        StartCoroutine(attackType.ToString());
    }

    public void StopFiring(AttackType attackType)
    {
        StopCoroutine(attackType.ToString());
    }

    private IEnumerator CircleFire()
    {
        float attackRate = 0.5f;
        int count = 25; // ⚠ 공격 양 줄임
        float intervalAngle = 360 / count;
        float weightAngle = 0;

        while (true)
        {
            foreach (Transform firePoint in FirePoint)
            {
                for (int i = 0; i < count; ++i)
                {
                    // ✅ 랜덤으로 프리팹 선택
                    GameObject prefab = projectilePrefabs[Random.Range(0, projectilePrefabs.Length)];
                    GameObject clone = Instantiate(prefab, firePoint.position, Quaternion.identity);

                    float angle = weightAngle + intervalAngle * i;
                    float x = Mathf.Cos(angle * Mathf.PI / 180.0f);
                    float y = Mathf.Sin(angle * Mathf.PI / 180.0f);

                    clone.GetComponent<Movement2D>().MoveTo(new Vector2(x, y));
                }
            }

            weightAngle += 5;
            yield return new WaitForSeconds(attackRate);
        }
    }

    private IEnumerator SingleFireToCenterPosition()
    {
        Vector3 targetPosition = Vector3.zero;
        float attackRate = 0.3f;

        while (true)
        {
            foreach (Transform firePoint in FirePoint)
            {
                GameObject prefab = projectilePrefabs[Random.Range(0, projectilePrefabs.Length)];
                GameObject clone = Instantiate(prefab, firePoint.position, Quaternion.identity);
                Vector3 direction = (targetPosition - firePoint.position).normalized;
                clone.GetComponent<Movement2D>().MoveTo(direction);
            }

            yield return new WaitForSeconds(attackRate);
        }
    }
}
