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
        _playerScript.isGrounded = Physics2D.OverlapCircle(
            _playerScript.groundCheckPoint.position, _playerScript.groundCheckRadius,
            _playerScript.whatIsGround);

    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        //kill player
        if (other.gameObject.CompareTag("OOBLeft"))
        {
            if (!_playerScript.isDead)
            {
                _playerScript.isDead = true;
                _playerScript.OnDeath();
            }
        }
        else if (other.gameObject.CompareTag("OOBRight"))
        {
            if (!_playerScript.isDead)
            {
                _playerScript.isDead = true;
                _playerScript.OnDeath();
            }
        }
        else if (other.gameObject.CompareTag("OOBTop"))
        {
            if (!_playerScript.isDead)
            {
                _playerScript.isDead = true;
                _playerScript.OnDeath();
            }
        }
        else if (other.gameObject.CompareTag("OOBBottom"))
        {
            if (!_playerScript.isDead)
            {
                _playerScript.isDead = true;
                _playerScript.OnDeath();
            }
        }
    }

    

    public void Knockback(Vector2 direction, float baseKB, float KBGrowth, float damage)
    {
        if (!_playerScript.isInvincible)
        {
            _playerScript.rb.AddForce(direction * (((_playerScript.health / 10) + (_playerScript.health * damage / 20) * KBGrowth) + baseKB), ForceMode2D.Impulse);
        }
    }
}
