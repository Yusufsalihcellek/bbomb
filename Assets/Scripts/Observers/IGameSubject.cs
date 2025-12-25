public interface IGameSubject
{
    // Abone Ekle
    void AddObserver(IGameObserver observer);
    // Abone Sil
    void RemoveObserver(IGameObserver observer);
    // Herkese Haber Ver
    void NotifyObservers(string eventName);
}