using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public GameObject creature;
    public GameObject aggroZone;

    [HideInInspector]
    public new Transform transform;
    [HideInInspector]
    public Rigidbody2D RB;
    [HideInInspector]
    public Animator anim;
    protected AudioSource _damagedSound;
    protected ParticleSystem _particles;

    [HideInInspector]
    public Transform _player;
    [HideInInspector]
    public Rigidbody2D _playerRB;

    [HideInInspector]
    public Vector3 startingPos;
    [HideInInspector]
    public Vector3 startingScale;

    public int maxHP;
    public int damage;
    public int _currentHP;
    public float speed;

    [HideInInspector]
    public float meleeSpeedIncrease = 1.5f;
    [HideInInspector]
    public float hitInvulDuration = 0.7f;
    //[HideInInspector]
    public float hitInvulTimer = 1f;
    [HideInInspector]
    public float stunPeriod = 0.15f;
    [HideInInspector]
    public float aggroLeashDuration = 5.0f;
    //[HideInInspector]
    public float aggroLeashTimer = 6.0f;
    [HideInInspector]
    public bool aggro = false;
    [HideInInspector]
    public bool playerInRange = false;
    //[HideInInspector]
    public bool isDead = false;
    //[HideInInspector]
    public bool isStunned = false;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        _damagedSound = GetComponent<AudioSource>();
        _particles = GetComponent<ParticleSystem>();

        transform = gameObject.transform;
        startingPos = transform.position;
        startingScale = transform.localScale;

        _currentHP = maxHP;
    }

    void Start()
    {
        _player = GameManager.GM.Player.transform;
        _playerRB = _player.GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (hitInvulTimer <= stunPeriod)
        {
            isStunned = true;
        }
        else
        {
            isStunned = false;
        }

        hitInvulTimer += Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" && GameManager.GM.Player.isGrounded == false && _playerRB.velocity.y <= 0)
        {
            Vector3 playerDistance = transform.position - _player.transform.position;
            if (playerDistance.x > 0)
                _player.transform.position += Vector3.left;
            else
                _player.transform.position += Vector3.right;
        }
    }


    public virtual void SetAggro(bool state)
    {
        aggro = state;
    }

    public virtual bool TakeDamage(int damage = 1)
    {
        SetAggro(true);

        if(hitInvulTimer > hitInvulDuration)
        {
            _damagedSound.Play();

            HealthManager.Instance.ChargeBerserk();

            hitInvulTimer = 0;

            _currentHP -= damage;
            if (_currentHP <= 0)
                TriggerDeath();

            return true;
        }

        return false;
    }

    protected virtual void TriggerDeath()
    {
        isDead = true;
    }
}

