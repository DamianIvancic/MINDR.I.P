using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    public GameObject creature;
    public GameObject aggroZone;

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
    public bool aggro = false;
    [HideInInspector]
    public float meleeSpeedIncrease = 1.5f;
    [HideInInspector]
    public bool isInRange = false;
    //[HideInInspector]
    public bool isDead = false;

    protected virtual void Awake()
    {
        _damagedSound = GetComponent<AudioSource>();
        _particles = GetComponent<ParticleSystem>();

        _transform = transform;
        startingPos = transform.position;
        startingScale = transform.localScale;

        _currentHP = maxHP;
    }


    public virtual void SetAggro(bool state)
    {
        aggro = state;
    }

    public void TakeDamage(int damage = 1)
    {
        _damagedSound.Play();

        _currentHP -= damage;
        if (_currentHP <= 0)
        {
            Kill();
            
        }
    }

    public void DamagePlayer(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Player")
        {
            HealthManager.Instance.TakeDamage(damage);
        }
    }

    public void DamageEnemy(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Weapon")
            TakeDamage();
    }

    protected virtual void Kill()
    {

    }
}

