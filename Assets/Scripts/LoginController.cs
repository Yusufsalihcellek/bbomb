using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    [Header("SAYFALAR (Paneller)")]
    public GameObject mainPanel;      // İlk açılan ekran (Seçim ekranı)
    public GameObject loginPanel;     // Giriş yapma formu
    public GameObject registerPanel;  // Kayıt olma formu

    [Header("GİRİŞ KUTULARI")]
    public InputField loginUserName;
    public InputField loginPassword;

    [Header("KAYIT KUTULARI")]
    public InputField registerUserName;
    public InputField registerPassword;

    [Header("DİĞER")]
    public Text feedbackText;

    private FileRepository _repo;

    private void Start()
    {
        _repo = new FileRepository();
        ShowMainPanel(); // Oyun açılınca Ana Menüyü göster
    }

    // --- SAYFA GEÇİŞLERİ ---
    public void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        feedbackText.text = "";
    }

    public void ShowLoginPanel()
    {
        mainPanel.SetActive(false);
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        feedbackText.text = "";
    }

    public void ShowRegisterPanel()
    {
        mainPanel.SetActive(false);
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        feedbackText.text = "";
    }

    // --- İŞLEMLER ---

    // Giriş Ekranındaki "GİRİŞ YAP" butonu buna bağlanacak
    public void PerformLogin()
    {
        string u = loginUserName.text;
        string p = loginPassword.text;

        if (_repo.LoginUser(u, p))
        {
            ShowMessage("Giriş Başarılı!", Color.green);
            SessionManager.Instance.playerName = u;
            Invoke("OpenGame", 1f);
        }
        else
        {
            ShowMessage("Hatalı İsim veya Şifre!", Color.red);
        }
    }

    // Kayıt Ekranındaki "KAYIT OL" butonu buna bağlanacak
    public void PerformRegister()
    {
        string u = registerUserName.text;
        string p = registerPassword.text;

        if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p))
        {
            ShowMessage("Alanları doldurun!", Color.yellow);
            return;
        }

        if (_repo.RegisterUser(u, p))
        {
            ShowMessage("Kayıt Oldun! Şimdi Giriş Yap.", Color.green);
            // Kayıt bitince otomatik Giriş ekranına atalım
            Invoke("ShowLoginPanel", 1.5f);
        }
        else
        {
            ShowMessage("Bu isim zaten alınmış.", Color.red);
        }
    }

    void OpenGame() => SceneManager.LoadScene("bomberman");

    void ShowMessage(string msg, Color col)
    {
        feedbackText.text = msg;
        feedbackText.color = col;
    }
}