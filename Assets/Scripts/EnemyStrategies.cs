using UnityEngine;

public interface IEnemyStrategy
{
    Vector2 GetDirection(); 
    void OnHitWall();
}

public class FoolStrategy : IEnemyStrategy
{
    private Vector2 _direction;
    private Vector2[] _allDirections = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    public FoolStrategy()
    {
        Debug.Log("Strateji Kuruluyor...");
        // İlk başta rastgele bir yön seç
        _direction = _allDirections[Random.Range(0, _allDirections.Length)];
        Debug.Log($"İlk Yön: {_direction}");
    }

    public Vector2 GetDirection()
    {
        // Yön asla (0,0) olamaz - güvenlik kontrolü
        if (_direction == Vector2.zero)
        {
            _direction = Vector2.right;
            Debug.LogWarning("Yön sıfırdı, sağa çevrildi!");
        }
        
        return _direction;
    }

    public void OnHitWall()
    {
        Debug.Log("Duvara Çarptı! Yön değiştiriliyor.");
        PickRandomDir();
    }

    private void PickRandomDir()
    {
        Vector2 oldDirection = _direction;
        Vector2 oppositeDirection = -_direction; // Geri gitmesin
        
        // Geri gitmeyecek şekilde yeni yön seç
        int maxAttempts = 10;
        int attempts = 0;
        
        do
        {
            _direction = _allDirections[Random.Range(0, _allDirections.Length)];
            attempts++;
            
            // Eski yön veya tam ters yön değilse kabul et
            if (_direction != oldDirection && _direction != oppositeDirection)
            {
                break;
            }
        }
        while (attempts < maxAttempts);
        
        // Eğer uygun yön bulunamadıysa, en azından geri gitme
        if (_direction == oppositeDirection)
        {
            // Sağa veya yukarı dön
            _direction = Random.value > 0.5f ? Vector2.right : Vector2.up;
        }
        
        Debug.Log($"Yeni Yön Seçildi: {_direction}");
    }
}

// İstersen Normal Strategy'yi de ekleyebiliriz
public class NormalStrategy : IEnemyStrategy
{
    private Vector2 _direction;
    private Vector2[] _allDirections = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    private float _changeDirectionTimer = 0f;
    private float _changeDirectionInterval = 2f; // 2 saniyede bir yön değiştir

    public NormalStrategy()
    {
        _direction = _allDirections[Random.Range(0, _allDirections.Length)];
    }

    public Vector2 GetDirection()
    {
        _changeDirectionTimer += Time.fixedDeltaTime;
        
        // Belirli aralıklarla rastgele yön değiştir
        if (_changeDirectionTimer >= _changeDirectionInterval)
        {
            _changeDirectionTimer = 0f;
            _direction = _allDirections[Random.Range(0, _allDirections.Length)];
        }
        
        return _direction;
    }

    public void OnHitWall()
    {
        // Tersi yöne git
        _direction = -_direction;
    }
}

// Akıllı düşman - oyuncuya doğru gider
public class CleverStrategy : IEnemyStrategy
{
    private Vector2 _direction;
    private Transform _enemyTransform;
    private Transform _playerTransform;

    public CleverStrategy(Transform enemy)
    {
        _enemyTransform = enemy;
        _direction = Vector2.right;
        
        // Oyuncuyu bul
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
    }

    public Vector2 GetDirection()
    {
        if (_playerTransform == null || _enemyTransform == null)
        {
            return _direction;
        }
        
        // Oyuncuya doğru git
        Vector2 toPlayer = (_playerTransform.position - _enemyTransform.position).normalized;
        
        // En baskın yönü seç
        if (Mathf.Abs(toPlayer.x) > Mathf.Abs(toPlayer.y))
        {
            _direction = toPlayer.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            _direction = toPlayer.y > 0 ? Vector2.up : Vector2.down;
        }
        
        return _direction;
    }

    public void OnHitWall()
    {
        // Duvara çarptıysa alternatif yöne git
        if (Mathf.Abs(_direction.x) > 0)
        {
            _direction = Random.value > 0.5f ? Vector2.up : Vector2.down;
        }
        else
        {
            _direction = Random.value > 0.5f ? Vector2.right : Vector2.left;
        }
    }
}