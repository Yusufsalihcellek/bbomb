using UnityEngine;

// Bu script sahneler değişse bile asla yok olmaz (Singleton + DontDestroyOnLoad)
public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;

    // Oyuncunun ismini burada tutacağız
    public string playerName = "Player 1"; 

    private void Awake()
    {
        // Sahnede sadece 1 tane SessionManager olsun
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değişince beni yok etme!
        }
        else
        {
            Destroy(gameObject);
        }
    }
}