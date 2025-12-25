using UnityEngine;

public class HardWall : MonoBehaviour
{
    [Header("Hasar Ayarları")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Görsel Değişim")]
    public Sprite[] damageSprites; // 3 sprite: tam, 1 hasar, 2 hasar
    private SpriteRenderer spriteRenderer;

    [Header("Kırılma")]
    public float destructionTime = 1f;
    [Range(0f, 1f)]
    public float itemSpawnChance = 0.3f;
    public GameObject[] spawnableItems;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        // İlk sprite'ı ayarla
        if (damageSprites.Length > 0 && spriteRenderer != null)
        {
            spriteRenderer.sprite = damageSprites[0];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Bomba patlaması mı? (Trigger collision)
        if (collision.CompareTag("Explosion"))
        {
            TakeDamage();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Bomba patlaması mı? (Normal collision)
        if (collision.gameObject.CompareTag("Explosion"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        currentHealth--;
        
        Debug.Log($"HardWall hasar aldı! Kalan can: {currentHealth}");

        // Sprite'ı güncelle (hasar durumuna göre)
        UpdateSprite();

        // Can bittiyse kır
        if (currentHealth <= 0)
        {
            DestroyWall();
        }
    }

    private void UpdateSprite()
    {
        if (spriteRenderer == null || damageSprites.Length == 0) return;

        // Can durumuna göre sprite seç
        int spriteIndex = maxHealth - currentHealth;
        
        // Index kontrolü
        if (spriteIndex >= 0 && spriteIndex < damageSprites.Length)
        {
            spriteRenderer.sprite = damageSprites[spriteIndex];
        }
    }

    private void DestroyWall()
    {
        Debug.Log("HardWall kırıldı!");

        // Item spawn et (şans varsa)
        if (spawnableItems.Length > 0 && Random.value < itemSpawnChance)
        {
            int randomIndex = Random.Range(0, spawnableItems.Length);
            Instantiate(spawnableItems[randomIndex], transform.position, Quaternion.identity);
        }

        // Duvarı yok et
        Destroy(gameObject, destructionTime);
    }
}