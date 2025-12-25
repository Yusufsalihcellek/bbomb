using UnityEngine;

public class PlayerDeadState : IPlayerState
{
    private MovementController _player;

    public PlayerDeadState(MovementController player)
    {
        _player = player;
    }

    // HandleInput ZORUNLU (Ölüler input almaz, boş bırakıyoruz)
    public void HandleInput() { }

    // UpdateState ZORUNLU
    public void UpdateState() { }
}