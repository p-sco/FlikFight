using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    public override void EnterState(PlayerController player)
    {
        player._animator.Play(player.PLAYER_JUMPING);
    }

    public override void OnCollisionEnter2D(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public override void Update(PlayerController player)
    {
        if (player._isInHitstun)
        {
            player.TransitionToState(player.HitstunState);
        }
        if (player._isGrounded && Mathf.Abs(player._rigidBody.velocity.x) < 0.01)
        {
            player.TransitionToState(player.IdleState);
        }
        if (player._isGrounded && Mathf.Abs(player._rigidBody.velocity.x) > 0.01)
        {
            player.TransitionToState(player.RunningState);
        }
    }
}
