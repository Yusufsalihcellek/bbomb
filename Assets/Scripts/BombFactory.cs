using UnityEngine;

public abstract class BombFactory : ScriptableObject
{
    // Bu satır olmazsa BombController çalışmaz!
    public abstract GameObject CreateBomb(Vector2 position);
}