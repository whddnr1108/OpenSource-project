using System.Collections;
using UnityEngine;

public enum AttackType { CircleFire = 0, SingleFireToCenterPosition }

public class BossWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject[] projectilePrefabs;

    [SerializeField]
    private Transform[] FirePoint;

    // 공격 타입별 데미지 설정
    [SerializeField]
    private int attack1Damage = 10;
    [SerializeField]
    private int attack2Damage = 5;
    // 필요하면 attack3Damage, attack4Damage 등 추가 가능

    private AttackType currentAttackType;

    public void StartFiring(AttackType attackType)
    {
        currentAttackType = attackType;
        StartCoroutine(attackType.ToString());
    }

    public void StopFiring(AttackType attackType)
    {
        StopCoroutine(attackType.ToString());
    }

    private GameObject GetRandomProjectile()
    {
        int index = Random.Range(0, projectilePrefabs.Length);
        return projectilePrefabs[index];
    }

    private IEnumerator CircleFire()
    {
        float attackRate = 0.5f;
        int count = 25;
        float intervalAngle = 360f / count;
        float weightAngle = 0;

        while (true)
        {
            foreach (Transform firePoint in FirePoint)
            {
                for (int i = 0; i < count; ++i)
                {
                    GameObject clone = Instantiate(GetRandomProjectile(), firePoint.position, Quaternion.identity);

                    var proj = clone.GetComponent<Projectile>();
                    if (proj != null)
                    {
                        proj.SetDamage(attack1Damage);
                    }

                    float angle = weightAngle + intervalAngle * i;
                    float x = Mathf.Cos(angle * Mathf.PI / 180f);
                    float y = Mathf.Sin(angle * Mathf.PI / 180f);

                    clone.GetComponent<Movement2D>().MoveTo(new Vector2(x, y));
                }
            }

            weightAngle += 1;
            yield return new WaitForSeconds(attackRate);
        }
    }

    private IEnumerator SingleFireToCenterPosition()
    {
        Vector3 targetPosition = Vector3.zero;
        float attackRate = 0.1f;

        while (true)
        {
            foreach (Transform firePoint in FirePoint)
            {
                GameObject clone = Instantiate(GetRandomProjectile(), firePoint.position, Quaternion.identity);

                var proj = clone.GetComponent<Projectile>();
                if (proj != null)
                {
                    proj.SetDamage(attack2Damage);
                }

                Vector3 direction = (targetPosition - firePoint.position).normalized;
                clone.GetComponent<Movement2D>().MoveTo(direction);
            }

            yield return new WaitForSeconds(attackRate);
        }
    }
}
