using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour, IGameObserver
{
    // STRUCTURAL PATTERN: Oyuncunun anlık bomba özellikleri burada tutuluyor
    public IBombStats currentBombStats = new BasicBombStats();
    public static GameManager Instance { get; private set; }
    public GameObject[] players;

    [Header("UI Ayarları")]
    public GameObject gameOverUI; // Game Over Paneli
    public Text highScoreText;    // Skor Yazısı

    // REPOSITORY PATTERN: Veritabanı arayüzü
    private IPlayerRepository _playerRepository;

    private void Awake()
    {
        if (Instance != null) { DestroyImmediate(gameObject); } 
        else { Instance = this; }

        // --- BAĞLANTI BURADA YAPILIYOR ---
        // PlayerPrefs veya DLL kullanmıyoruz. 
        // Yazdığımız 'FileRepository' sistemini başlatıyoruz.
        _playerRepository = new FileRepository();
    }

    private void OnDestroy() { if (Instance == this) { Instance = null; } }

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerObj in players)
        {
            MovementController playerController = playerObj.GetComponent<MovementController>();
            if (playerController != null)
            {
                playerController.AddObserver(this);
            }
        }
    }

    public void OnNotify(string eventName, object data)
    {
        if (eventName == "PlayerDied")
        {
            CheckWinState();
        }
    }

   public void CheckWinState()
    {
        int aliveCount = 0;
        GameObject lastAlivePlayer = null;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].activeSelf) {
                aliveCount++;
                lastAlivePlayer = players[i];
            }
        }

        if (aliveCount <= 1) 
        {
            // --- BURAYI DEĞİŞTİRİYORUZ ---
            // Eğer kazanan Player 1 ise (Bizim girdiğimiz isim)
            // Player objelerinin adlarını Unity'de "Player 1" ve "Player 2" yaptığını varsayıyorum.
            
            string winnerName = "Bilinmiyor";

            if (lastAlivePlayer != null)
            {
                // Eğer kazanan Player 1 ise giriş ekranındaki ismi kullan
                if (lastAlivePlayer.name == "Player 1") 
                {
                    winnerName = SessionManager.Instance.playerName;
                }
                else
                {
                    winnerName = lastAlivePlayer.name; // Player 2 vs.
                }
            }
            
            Debug.Log("Kazanan: " + winnerName);

            // Veritabanına kaydet
            if (_playerRepository != null)
            {
                _playerRepository.SaveWin();
            }

            // Ekranda İsmi Göstermek İstersen HighScoreText'i geçici olarak buna çevirebiliriz
            if (highScoreText != null)
            {
                highScoreText.text = winnerName + " KAZANDI!";
            }

            Invoke(nameof(GameOver), 2f); // Biraz daha uzun bekletelim yazıyı okusunlar
        }
    }
    private void GameOver()
    {
        if (gameOverUI != null)
        {
            // Veritabanından skoru çekip ekrana yazalım
            if (_playerRepository != null && highScoreText != null)
            {
                int score = _playerRepository.GetHighScore();
                highScoreText.text = "TOPLAM ZAFER: " + score;
            }

            gameOverUI.SetActive(true);
        }
        else
        {
            Invoke(nameof(NewRound), 3f);
        }
    }

    private void NewRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}