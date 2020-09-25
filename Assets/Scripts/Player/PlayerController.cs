using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //Game variables
    public float maxHealth = 999.9f;
    public int lives = 3;
    public float health;
    //[SerializeField]
    public Color playerColor;

    //store a reference to all the sub player scripts
    [SerializeField]
    internal PlayerInputScript _inputScript;
    [SerializeField]
    internal PlayerCollisionScript _collisionScript;
    [SerializeField]
    internal PlayerMovementScript _movementScript;

    //child overrides
    [SerializeField]
    internal float moveSpeed;
    [SerializeField]
    internal float jumpForce;
    [SerializeField]
    internal float attackSpeed;
    [SerializeField]
    internal float spikeDamage = 10f;
    [SerializeField]
    internal float swipeDamage = 8f;
    [SerializeField]
    internal float weight;
    [SerializeField]
    internal float fastFallSpeed;


    //physics
    internal Rigidbody2D rb;
    private Vector2 velocity;
    private Vector2 direction;


    [SerializeField]
    internal Transform groundCheckPoint;
    [SerializeField]
    internal float groundCheckRadius;
    [SerializeField]
    internal bool isGrounded;
    [SerializeField]
    internal LayerMask whatIsGround;

    //states
    internal bool isInvincible;
    internal bool isDead = false;
    internal bool isKnocked;

    internal bool canDoubleJump;
    internal bool jump;

    internal bool isInHitstun;
    internal float hitstunDuration = 0f; 
    internal float hitstunTimer;
    internal bool facingRight;

    internal float attackTimer = 0;
    internal float attackCooldown = 0.4f;
    internal bool isAttacking = false;

    internal float invincibilityTimer = 0;
    internal float invincibilityDuration = 2.0f;

    //animator
    internal Animator anim;
    internal SpriteRenderer renderers;
    internal SwipeDirection dir;

    internal int player;

    protected void Awake() {
        
    }

    private void Start()
    {
        print("PlayerControllerScript starting");

        isInvincible = false;
        health = 0;
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        renderers = gameObject.GetComponent<SpriteRenderer>();
        //move this logic to player spawner
        if (gameObject.name == "Player1") {
            player = 1;
            playerColor = Color.red;
            renderers.material.SetColor("_Color", playerColor);
            facingRight = true;
        }
        if (gameObject.name == "Player2") {
            player = 2;
            playerColor = Color.blue;
            renderers.material.SetColor("_Color", playerColor);
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    protected void Update() {
        
    }

    public bool getIsGrounded()
    {
        return isGrounded;
    }

    public void TakeDamage(float damageReceived) {
        if (!isInvincible) {
            health += damageReceived;
            isInHitstun = true;
            hitstunDuration = (health/(damageReceived*10));
            hitstunTimer = hitstunDuration;
            invincibilityTimer = 0.1f;
            StartCoroutine(HitFlash());
        }
    }

    public void OnDeath() {
        //set to respawn point for P1 and P2
        //respawn player with invincible frames

        if (gameObject.name == "Player1") {
            MatchStats.Instance.P1DmgReceived+=health;
            MatchStats.Instance.P2DmgDealt+=health;
        }
        if (gameObject.name == "Player2") {
            MatchStats.Instance.P2DmgReceived += health;
            MatchStats.Instance.P1DmgDealt += health;
        }

        health = 0;
        --lives;
        StartCoroutine(ResetIsDead());
    }

    public IEnumerator ResetIsDead() {
        yield return new WaitForEndOfFrame();
        //reset player traits
        isDead = false;
        GameMaster.KillPlayer(this);
        hitstunTimer = 0;
        //disable weapon child box colliders
        gameObject.transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = false;
        gameObject.transform.GetChild(2).GetComponent<BoxCollider2D>().enabled = false;
    }

    public void OnRespawn() {
        StartCoroutine(InvincibilityFlash());
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
    }

    

    public IEnumerator HitFlash() {
        Color32 whateverColor = new Color32(255, 182, 182, 255); //edit r,g,b and the alpha values to what you want
        for (var n = 0; n < hitstunDuration; n++) {
            renderers.material.color = whateverColor;
            yield return new WaitForSeconds(0.1f);
            renderers.material.color = playerColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator InvincibilityFlash() {
        Color32 whateverColor = new Color32(255, 182, 182, 255); //edit r,g,b and the alpha values to what you want
        for (var n = 0; n < 10; n++) {
            renderers.material.color = whateverColor;
            yield return new WaitForSeconds(0.1f);
            renderers.material.color = playerColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    

    public void SpikeAttack() {
        if (!isAttacking && !isInHitstun) {
            anim.SetTrigger("SpikeAttack");
            isAttacking = true;
            attackTimer = attackCooldown;
        }
    }

    public void SwipeAttack() {
        if (!isInHitstun && !isAttacking) {
            anim.SetTrigger("SwipeAttack");
            isAttacking = true;
            attackTimer = attackCooldown;
        }
    }

}
