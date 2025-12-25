public interface IGameObserver
{
    // Gözlemciye "Bir şey oldu" deme fonksiyonu
    void OnNotify(string eventName, object data);
}