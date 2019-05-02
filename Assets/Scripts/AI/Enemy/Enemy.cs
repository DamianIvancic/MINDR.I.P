using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public GameObject creature;
    public GameObject aggroZone;

    [HideInInspector]
    public Rigidbody2D RB;
    [HideInInspector]
    public Animator anim;
    protected AudioSource _damagedSound;
    protected ParticleSystem _particles;

    protected Transform _transform;

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

        _transform = transform;
        startingPos = transform.position;
        startingScale = transform.localScale;

        _currentHP = maxHP;
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

    public virtual void SetAggro(bool state)
    {
        aggro = state;
    }

    public void TakeDamage(int damage = 1)
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
        }  
    }

    protected virtual void TriggerDeath()
    {}
}

