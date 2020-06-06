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
    private bool isInvincible;
    private bool isDead = false;

    //[SerializeField]
    public Color playerColor;

    //child overrides
    public float moveSpeed;
    public float jumpForce;
    public float attackSpeed;
    public float spikeDamage = 10f;
    public float swipeDamage = 8f;
    public float weight;
    public float fastFallSpeed;

    protected SwipeDirection dir;

    //physics
    private Rigidbody2D rb;
    private Vector2 velocity;
    private Vector2 direction;
    public bool isKnocked;
    public Transform groundCheckPoint;
    public float groundCheckRadius;
    protected bool isGrounded;
    public LayerMask whatIsGround;
    protected bool canDoubleJump;
    protected bool jump;
    protected bool isInHitstun;
    protected float hitstunDuration = 0f; 
    protected float hitstunTimer;

    //animator
    public Animator anim;
    private bool facingRight;
    protected SpriteRenderer renderers;

    private float attackTimer = 0;
    private float attackCd = 0.4f;
    public bool isAttacking = false;

    public float invincibilityTimer = 0;
    protected float invincibilityDuration = 2.0f;

    protected void Start() {
        isInvincible = false;
        health = 0;
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        renderers = gameObject.GetComponent<SpriteRenderer>();

        //move this logic to player spawner
        if (gameObject.name == "Player1") {
            playerColor = Color.red;
            renderers.material.SetColor("_Color", playerColor);
            facingRight = true;
        }
        if (gameObject.name == "Player2") {
            playerColor = Color.blue;
            renderers.material.SetColor("_Color", playerColor);
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    protected void Update() {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);
        anim.SetFloat("Velocity.x", Mathf.Abs(rb.velocity.x));
        anim.SetBool("IsInHitstun", isInHitstun);
        anim.SetBool("Grounded", isGrounded);

        if (isGrounded) {
            canDoubleJump = true;
        }
        if (dir == SwipeDirection.Up) {
            jump = true;
        }

        if (isAttacking) {
            if (attackTimer > 0) {
                attackTimer -= Time.deltaTime;
            } else {
                isAttacking = false;
            }
        }

        if (isInHitstun) {
            if (hitstunTimer > 0) {
                hitstunTimer -= Time.deltaTime;
            } else {
                isInHitstun = false;
            }
        }

        if (isInvincible) {
            if (invincibilityTimer > 0) {
                invincibilityTimer -= Time.deltaTime;
            } else {
                isInvincible = false;
            }
        }
    }

    private void FixedUpdate() {
        if (!isInHitstun) {
            if (jump) {
                if (isGrounded) {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    canDoubleJump = true;
                } else {
                    if (canDoubleJump) {
                        canDoubleJump = false;
                        rb.velocity = new Vector2(rb.velocity.x, 0);
                        rb.velocity = new Vector2(rb.velocity.x, jumpForce / 1);
                    }
                }
                dir = SwipeDirection.None;
                jump = false;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {

        //kill player
        if (other.gameObject.CompareTag("OOBLeft")) {
            if (!isDead) {
                isDead = true;
                OnDeath();
            }
        } else if (other.gameObject.CompareTag("OOBRight")) {
            if (!isDead) {
                isDead = true;
                OnDeath();
            }
        } else if (other.gameObject.CompareTag("OOBTop")) {
            if (!isDead) {
                isDead = true;
                OnDeath();
            }
        } else if (other.gameObject.CompareTag("OOBBottom")) {
            if (!isDead) {
                isDead = true;
                OnDeath();
            }
        }
    }

    private void Flip() {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
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
        isDead = false;
        GameMaster.KillPlayer(this);
        hitstunTimer = 0;
        gameObject.transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = false;
        gameObject.transform.GetChild(2).GetComponent<BoxCollider2D>().enabled = false;
    }

    public void OnRespawn() {
        StartCoroutine(InvincibilityFlash());
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
    }

    public void Knockback(Vector2 direction, float baseKB, float KBGrowth, float damage) {
        if (!isInvincible) {
            rb.AddForce(direction * (((health/10)+(health*damage/20)*KBGrowth) + baseKB), ForceMode2D.Impulse);
        }
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

    public void ReceiveAction(SwipeDirection direction) {
        dir = direction;
        if (!isInHitstun) {
            if (dir == SwipeDirection.Left) {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                if (facingRight) {
                    Flip();
                }
            } else if (dir == SwipeDirection.Right) {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                if (!facingRight) {
                    Flip();
                }
            } else if (dir == SwipeDirection.Down) {
                rb.velocity = new Vector2(0, -fastFallSpeed);
            }
        }
    }

    public void SpikeAttack() {
        if (!isAttacking && !isInHitstun) {
            anim.SetTrigger("SpikeAttack");
            isAttacking = true;
            attackTimer = attackCd;
        }
    }

    public void SwipeAttack() {
        if (!isInHitstun && !isAttacking) {
            anim.SetTrigger("SwipeAttack");
            isAttacking = true;
            attackTimer = attackCd;
        }
    }

    public bool getIsGrounded() {
        return isGrounded;
    }

}
