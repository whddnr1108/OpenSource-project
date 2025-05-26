using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // PlayerHP 컴포넌트를 가져와서 데미지를 줌
            PlayerHP playerHP = collision.GetComponent<PlayerHP>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(damage);
            }

            // 발사체 제거
            Destroy(gameObject);
        }
        
    }
}
