using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    [SerializeField]
    PlayerController _playerScript;

    // Start is called before the first frame update
    void Start()
    {
        print("PlayerMovementScript starting");
    }

    // Update is called once per frame
    void Update()
    {
        _playerScript.anim.SetFloat("Velocity.x", Mathf.Abs(_playerScript.rb.velocity.x));
        _playerScript.anim.SetBool("IsInHitstun", _playerScript.isInHitstun);
        _playerScript.anim.SetBool("Grounded", _playerScript.isGrounded);

        if (_playerScript.isGrounded)
        {
            _playerScript.canDoubleJump = true;
        }
        if (_playerScript.dir == SwipeDirection.Up)
        {
            _playerScript.jump = true;
        }

        if (_playerScript.isAttacking)
        {
            if (_playerScript.attackTimer > 0)
            {
                _playerScript.attackTimer -= Time.deltaTime;
            }
            else
            {
                _playerScript.isAttacking = false;
            }
        }

        if (_playerScript.isInHitstun)
        {
            if (_playerScript.hitstunTimer > 0)
            {
                _playerScript.hitstunTimer -= Time.deltaTime;
            }
            else
            {
                _playerScript.isInHitstun = false;
            }
        }

        if (_playerScript.isInvincible)
        {
            if (_playerScript.invincibilityTimer > 0)
            {
                _playerScript.invincibilityTimer -= Time.deltaTime;
            }
            else
            {
                _playerScript.isInvincible = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_playerScript.isInHitstun)
        {
            if (_playerScript.jump)
            {
                if (_playerScript.isGrounded)
                {
                    _playerScript.rb.velocity = new Vector2(_playerScript.rb.velocity.x, _playerScript.jumpForce);
                    _playerScript.canDoubleJump = true;
                }
                else
                {
                    if (_playerScript.canDoubleJump)
                    {
                        _playerScript.canDoubleJump = false;
                        _playerScript.rb.velocity = new Vector2(_playerScript.rb.velocity.x, 0);
                        _playerScript.rb.velocity = new Vector2(_playerScript.rb.velocity.x, _playerScript.jumpForce / 1);
                    }
                }
                _playerScript.dir = SwipeDirection.None;
                _playerScript.jump = false;
            }
        }
    }

    public void ReceiveAction(SwipeDirection direction)
    {
        _playerScript.dir = direction;
        if (!_playerScript.isInHitstun)
        {
            if (_playerScript.dir == SwipeDirection.Left)
            {
                _playerScript.rb.velocity = new Vector2(-_playerScript.moveSpeed, _playerScript.rb.velocity.y);
                if (_playerScript.facingRight)
                {
                    Flip();
                }
            }
            else if (_playerScript.dir == SwipeDirection.Right)
            {
                _playerScript.rb.velocity = new Vector2(_playerScript.moveSpeed, _playerScript.rb.velocity.y);
                if (!_playerScript.facingRight)
                {
                    Flip();
                }
            }
            else if (_playerScript.dir == SwipeDirection.Down)
            {
                _playerScript.rb.velocity = new Vector2(0, -_playerScript.fastFallSpeed);
            }
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        _playerScript.facingRight = !_playerScript.facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
