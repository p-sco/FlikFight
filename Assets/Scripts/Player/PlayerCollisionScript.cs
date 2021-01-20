using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionScript : MonoBehaviour
{
    [SerializeField]
    PlayerController _playerScript;

    // Start is called before the first frame update
    void Start()
    {
        print("PlayerCollisionScript starting");
        
    }

    // Update is called once per frame
    void Update()
    {
        _playerScript._isGrounded = Physics2D.OverlapCircle(
            _playerScript._groundCheckPoint.position, _playerScript._groundCheckRadius,
            _playerScript._whatIsGround);

    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        //kill player
        if (other.gameObject.CompareTag("OOBLeft"))
        {
            if (!_playerScript._isDead)
            {
                _playerScript._isDead = true;
                _playerScript.OnDeath();
            }
        }
        else if (other.gameObject.CompareTag("OOBRight"))
        {
            if (!_playerScript._isDead)
            {
                _playerScript._isDead = true;
                _playerScript.OnDeath();
            }
        }
        else if (other.gameObject.CompareTag("OOBTop"))
        {
            if (!_playerScript._isDead)
            {
                _playerScript._isDead = true;
                _playerScript.OnDeath();
            }
        }
        else if (other.gameObject.CompareTag("OOBBottom"))
        {
            if (!_playerScript._isDead)
            {
                _playerScript._isDead = true;
                _playerScript.OnDeath();
            }
        }
    }

    

    public void Knockback(Vector2 direction, float baseKnockback, float knockbackGrowth, float damage, Vector2 directionalInfluence)
    {
        if (!_playerScript._isInvincible)
        {
            _playerScript._rigidBody.AddForce(direction * (((_playerScript.Health / 10) + 
                (_playerScript.Health * damage / 20) * knockbackGrowth) + baseKnockback), ForceMode2D.Impulse);
        }
    }
}
