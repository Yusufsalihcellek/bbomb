using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    public LayerMask wallLayer; // Inspector'dan Wall layer'ı seç
    
    private IEnemyStrategy _moveStrategy;
    private Rigidbody2D _rb;
    private Vector2 _currentDirection;
    private float _gridSize = 1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f; 
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.freezeRotation = true;
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Start()
    {
        _moveStrategy = new FoolStrategy();
        _currentDirection = _moveStrategy.GetDirection();
        
        // Başlangıç pozisyonunu hizala
        Vector2 pos = transform.position;
        transform.position = new Vector2(
            Mathf.Round(pos.x), 
            Mathf.Round(pos.y)
        );
    }

    private void FixedUpdate()
    {
        if (_moveStrategy != null)
        {
            // Önümüzde duvar var mı kontrol et (0.6 birim ileride)
            RaycastHit2D hit = Physics2D.Raycast(
                _rb.position, 
                _currentDirection, 
                0.6f, 
                wallLayer
            );
            
            // Duvar varsa yön değiştir
            if (hit.collider != null)
            {
                HandleWallDetection();
            }
            
            // Hareketi uygula
            _rb.linearVelocity = _currentDirection * speed;
        }
    }

    private void HandleWallDetection()
    {
        Debug.Log($"Duvar algılandı! Eski yön: {_currentDirection}");
        
        // 1. Pozisyonu grid'e hizala
        Vector2 currentPos = _rb.position;
        float newX = Mathf.Round(currentPos.x);
        float newY = Mathf.Round(currentPos.y);
        _rb.position = new Vector2(newX, newY);
        
        Debug.Log($"Grid'e hizalandı: {_rb.position}");
        
        // 2. Yeni yön seç
        _moveStrategy.OnHitWall();
        _currentDirection = _moveStrategy.GetDirection();
        
        Debug.Log($"Yeni yön: {_currentDirection}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Sadece player collision'ı için
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<MovementController>();
            if (player != null)
            {
                player.DeathSequence();
            }
        }
    }
}