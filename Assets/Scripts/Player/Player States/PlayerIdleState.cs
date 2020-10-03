using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    
    public override void EnterState(PlayerController player)
    {
        player._animator.Play(player.PLAYER_IDLE);
    }

    public override void OnCollisionEnter2D(PlayerController player)
    {

    }

    public override void Update(PlayerController player)
    {
        if (player._isInHitstun)
        {
            player.TransitionToState(player.HitstunState);
        }
        if (player._isJumping)
        {
            player.TransitionToState(player.JumpingState);
        }
        if (Mathf.Abs(player._rigidBody.velocity.x) > 0.01)
        {
            player.TransitionToState(player.RunningState);
        }
    }
}
