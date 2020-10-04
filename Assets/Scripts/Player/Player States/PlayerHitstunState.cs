using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitstunState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        player._animator.Play(player.PLAYER_IN_HITSTUN);
    }

    public override void OnCollisionEnter2D(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public override void Update(PlayerController player)
    {
        if (!player._isInHitstun)
        {
            player.TransitionToState(player.IdleState);
        }
    }
}
