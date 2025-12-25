using UnityEngine;

public class PlayerAliveState : IPlayerState
{
    private MovementController _player;

    public PlayerAliveState(MovementController player)
    {
        _player = player;
    }

    // HandleInput ZORUNLU
    public void HandleInput()
    {
        if (_player != null)
        {
            _player.HandleInput();
        }
    }

    // UpdateState ZORUNLU (İçi boş olsa bile yazılmalı)
    public void UpdateState() { }
}