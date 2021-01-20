using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;
using Helper = Assets.Scripts.Helper;
public class PlayerController : MonoBehaviour
{

    //Game variables
    public float MaxHealth = 999.9f;
    public int Lives = 3;
    public float Health = 0f;
    //[SerializeField]
    public Color PlayerColor;

    //store a reference to all the sub player scripts
    [SerializeField]
    internal PlayerInputScript _inputScript;
    [SerializeField]
    internal PlayerCollisionScript _collisionScript;
    [SerializeField]
    internal PlayerMovementScript _movementScript;

    //child overrides
    [SerializeField]
    internal float _moveSpeed;
    [SerializeField]
    internal float _jumpForce;
    [SerializeField]
    internal float _attackSpeed;
    [SerializeField]
    internal float _spikeDamage = 10f;
    [SerializeField]
    internal float _swipeDamage = 8f;
    [SerializeField]
    internal float _weight;
    [SerializeField]
    internal float _fastFallSpeed;


    //physics
    internal Rigidbody2D _rigidBody;
    private Vector2 _velocity;
    private Vector2 _direction;


    [SerializeField]
    internal Transform _groundCheckPoint;
    [SerializeField]
    internal float _groundCheckRadius;
    [SerializeField]
    internal bool _isGrounded;
    [SerializeField]
    internal LayerMask _whatIsGround;

    
    //state variables
    internal bool _isInvincible;
    internal bool _isDead = false;

    internal bool _canDoubleJump;
    internal bool _isJumping;

    internal bool _isInHitstun;
    internal bool _isInHitLag;
    internal float _hitstunDuration = 0f; 
    internal float _hitLagDuration = 0f; 
    internal float _hitstunTimer;
    internal Vector2 _bufferedDirection;
    internal bool _facingRight;

    internal float _attackCooldown;
    internal bool _isAttacking = false;

    internal float _invincibilityTimer = 0;
    internal float _invincibilityDuration = 2.0f;

    //animator
    internal SpriteRenderer _renderers;
    internal Animator _animator;
    internal readonly string PLAYER_IDLE = "Player_Idle";
    internal readonly string PLAYER_RUNNING = "Player_Running";
    internal readonly string PLAYER_JUMPING = "Player_Jumping";
    internal readonly string PLAYER_IN_HITSTUN = "Player_InHitstun";
    internal readonly string PLAYER_IS_KNOCKED = "Player_IsKnocked";
    internal readonly string PLAYER_FORWARD_ATTACK = "Player_ForwardAttack";
    internal readonly string PLAYER_SPIKE_ATTACK = "Player_SpikeAttack";


    internal SwipeDirection Direction;
    internal int _playerId;

    //states
    public readonly PlayerIdleState IdleState = new PlayerIdleState();
    public readonly PlayerJumpingState JumpingState = new PlayerJumpingState();
    public readonly PlayerRunningState RunningState = new PlayerRunningState();
    public readonly PlayerHitstunState HitstunState = new PlayerHitstunState();
    public readonly PlayerForwardAttackState ForwarAttackState = new PlayerForwardAttackState();
    public readonly PlayerSpikeAttackState SpikeAttackState = new PlayerSpikeAttackState();
    private PlayerBaseState _currentState;

    public PlayerBaseState CurrentState
    {
        get { return CurrentState; }
    }

    protected void Awake() {
        
    }

    private void Start()
    {
        print("PlayerControllerScript starting");

        _isInvincible = false;
        Health = 0;
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        _animator = gameObject.GetComponent<Animator>();
        _renderers = gameObject.GetComponent<SpriteRenderer>();
        //move this logic to player spawner
        if (gameObject.name == "Player1") {
            _playerId = 1;
            PlayerColor = Color.red;
            _renderers.material.SetColor("_Color", PlayerColor);
            _facingRight = true;
        }
        if (gameObject.name == "Player2") {
            _playerId = 2;
            PlayerColor = Color.blue;
            _renderers.material.SetColor("_Color", PlayerColor);
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        TransitionToState(IdleState);
    }

    protected void Update() {
        _currentState.Update(this);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    _currentState.OnCollisionEnter2D(this);
    //}

    public void TransitionToState(PlayerBaseState state)
    {
        _currentState = state;
        _currentState.EnterState(this);
    }

    public bool getIsGrounded()
    {
        return _isGrounded;
    }

    public void TakeDamage(float damageReceived) {
        if (!_isInvincible) {
            Health += damageReceived;
            _isInHitstun = true;
            _hitstunDuration = (Health/(damageReceived*10));
            _hitstunTimer = _hitstunDuration;
            _invincibilityTimer = 0.1f;
            StartCoroutine(HitFlash());
        }
    }

    public void OnDeath() {
        //set to respawn point for P1 and P2
        //respawn player with invincible frames

        if (gameObject.name == "Player1") {
            MatchStats.Instance.P1DmgReceived += Health;
            MatchStats.Instance.P2DmgDealt += Health;
        }
        if (gameObject.name == "Player2") {
            MatchStats.Instance.P2DmgReceived += Health;
            MatchStats.Instance.P1DmgDealt += Health;
        }

        Health = 0;
        --Lives;
        StartCoroutine(ResetIsDead());
    }

    public IEnumerator ResetIsDead() {
        yield return new WaitForEndOfFrame();
        //reset player traits
        _isDead = false;
        GameMaster.KillPlayer(this);
        _hitstunTimer = 0;
        //disable weapon child box colliders
        gameObject.transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = false;
        gameObject.transform.GetChild(2).GetComponent<BoxCollider2D>().enabled = false;
    }

    public void OnRespawn() {
        StartCoroutine(InvincibilityFlash());
        _isInvincible = true;
        _invincibilityTimer = _invincibilityDuration;
    }

    public IEnumerator HitFlash() {
        Color32 whateverColor = new Color32(255, 182, 182, 255); //edit r,g,b and the alpha values to what you want
        for (var n = 0; n < _hitstunDuration; n++) {
            _renderers.material.color = whateverColor;
            yield return new WaitForSeconds(0.1f);
            _renderers.material.color = PlayerColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator InvincibilityFlash() {
        Color32 whateverColor = new Color32(255, 182, 182, 255); //edit r,g,b and the alpha values to what you want
        for (var n = 0; n < 10; n++) {
            _renderers.material.color = whateverColor;
            yield return new WaitForSeconds(0.1f);
            _renderers.material.color = PlayerColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SpikeAttack() {
        if (!_isAttacking && !_isInHitstun && !_isInHitLag) {
            _isAttacking = true;
            TransitionToState(SpikeAttackState);
            _attackCooldown = _animator.GetCurrentAnimatorStateInfo(0).length;
        }
    }

    public void SwipeAttack() {
        if (!_isInHitstun && !_isAttacking && !_isInHitLag) {
            _isAttacking = true;
            TransitionToState(ForwarAttackState);
            _attackCooldown = _animator.GetCurrentAnimatorStateInfo(0).length;
        }
    }

}
