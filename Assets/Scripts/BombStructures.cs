using UnityEngine;

// 1. INTERFACE (Temel Özellikler)
public interface IBombStats
{
    int GetRadius(); // Patlama menzili kaç?
}

// 2. COMPONENT (Varsayılan Bomba)
public class BasicBombStats : IBombStats
{
    public int GetRadius()
    {
        return 1; // Standart menzil 1 kare
    }
}

// 3. DECORATOR BASE (Süsleyici Temeli)
public abstract class BombDecorator : IBombStats
{
    protected IBombStats _wrappedBomb; // İçindeki asıl özellik

    public BombDecorator(IBombStats bomb)
    {
        _wrappedBomb = bomb;
    }

    public virtual int GetRadius()
    {
        return _wrappedBomb.GetRadius();
    }
}

// 4. CONCRETE DECORATOR (Özel Süsleyici: Menzil Arttıran)
public class RadiusEnhancer : BombDecorator
{
    public RadiusEnhancer(IBombStats bomb) : base(bomb) { }

    public override int GetRadius()
    {
        // Mevcut menzile +1 ekle
        return _wrappedBomb.GetRadius() + 1;
    }
}
