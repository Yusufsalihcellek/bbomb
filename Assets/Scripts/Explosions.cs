using UnityEngine;

public class Explosions : MonoBehaviour
{
    public AnimatedSpriteRenderer start;
    public AnimatedSpriteRenderer middle;
    public AnimatedSpriteRenderer end;

    public void SetActiveRenderer(AnimatedSpriteRenderer renderer)
    {
        start.enabled = renderer == start;
        middle.enabled = renderer == middle;
        end.enabled = renderer == end;
    }

    public void SetDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }

    // --- KRİTİK GÜNCELLEME BURADA ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eğer patlama OYUNCUYA değerse
        if (other.CompareTag("Player"))
        {
            // Oyuncuyu öldür (Observer pattern tetiklenir)
            other.gameObject.GetComponent<MovementController>().DeathSequence();
        }
        
        // Eğer patlama DÜŞMANA değerse
        else if (other.CompareTag("Enemy"))
        {
            Debug.Log("Düşman vuruldu!");
            // Düşmanı sahneden sil
            Destroy(other.gameObject);
            
            // İstersen buraya düşman ölme efekti veya ses ekleyebilirsin
        }
    }
}