public interface IPlayerRepository
{
    // Veritabanından en yüksek skoru getir
    int GetHighScore();

    // Veritabanına yeni zaferi kaydet
    void SaveWin();
}