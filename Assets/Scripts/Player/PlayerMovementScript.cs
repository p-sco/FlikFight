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
        //_playerScript.anim.SetFloat("Velocity.x", Mathf.Abs(_playerScript.rb.velocity.x));
        //_playerScript.anim.SetBool("IsInHitstun", _playerScript.isInHitstun);
        //_playerScript.anim.SetBool("Grounded", _playerScript.isGrounded);

        if (_playerScript._isGrounded)
        {
            _playerScript._canDoubleJump = true;
        }
        if (_playerScript.Direction == SwipeDirection.Up)
        {
            _playerScript._isJumping = true;
        }

        if (_playerScript._isAttacking)
        {
            if (_playerScript._attackCooldown > 0)
            {
                _playerScript._attackCooldown -= Time.deltaTime;
            }
            else
            {
                _playerScript._isAttacking = false;
            }
        }

        if (_playerScript._isInHitstun)
        {
            if (_playerScript._hitstunTimer > 0)
            {
                _playerScript._hitstunTimer -= Time.deltaTime;
            }
            else
            {
                _playerScript._isInHitstun = false;
            }
        }

        if (_playerScript._isInvincible)
        {
            if (_playerScript._invincibilityTimer > 0)
            {
                _playerScript._invincibilityTimer -= Time.deltaTime;
            }
            else
            {
                _playerScript._isInvincible = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_playerScript._isInHitstun)
        {
            if (_playerScript._isJumping)
            {
                if (_playerScript._isGrounded)
                {
                    _playerScript._rigidBody.velocity = new Vector2(_playerScript._rigidBody.velocity.x,
                        _playerScript._jumpForce);
                    _playerScript._canDoubleJump = true;
                }
                else
                {
                    if (_playerScript._canDoubleJump)
                    {
                        _playerScript._canDoubleJump = false;
                        _playerScript._rigidBody.velocity = new Vector2(_playerScript._rigidBody.velocity.x, 0);
                        _playerScript._rigidBody.velocity = new Vector2(_playerScript._rigidBody.velocity.x,
                            _playerScript._jumpForce / 1);
                    }
                }
                _playerScript.Direction = SwipeDirection.None;
                _playerScript._isJumping = false;
            }
        }
    }

    public void ReceiveAction(SwipeDirection direction)
    {
        _playerScript.Direction = direction;
        if (!_playerScript._isInHitstun)
        {
            if (_playerScript.Direction == SwipeDirection.Left)
            {
                _playerScript._rigidBody.velocity = new Vector2(-_playerScript._moveSpeed,
                    _playerScript._rigidBody.velocity.y);

                if (_playerScript._facingRight)
                {
                    Flip();
                }
            }
            else if (_playerScript.Direction == SwipeDirection.Right)
            {
                _playerScript._rigidBody.velocity = new Vector2(_playerScript._moveSpeed, 
                    _playerScript._rigidBody.velocity.y);

                if (!_playerScript._facingRight)
                {
                    Flip();
                }
            }
            else if (_playerScript.Direction == SwipeDirection.Down)
            {
                _playerScript._rigidBody.velocity = new Vector2(0, -_playerScript._fastFallSpeed);
            }
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        _playerScript._facingRight = !_playerScript._facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
