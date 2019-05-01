using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    public GameObject creature;
    public GameObject aggroZone;

    private AudioSource _damagedSound;
    private ParticleSystem _particles;

    private Transform _transform;
 
    [HideInInspector]
    public Vector3 startingPos;
    [HideInInspector]
    public Vector3 startingScale;

    public int maxHP;
    public int damage;
    private int _currentHP;
    public float speed;

    [HideInInspector]
    public bool aggro;

    protected virtual void Awake()
    {
        _damagedSound = GetComponent<AudioSource>();
        _particles = GetComponent<ParticleSystem>();

        _transform = transform;
        startingPos = transform.position;
        startingScale = transform.localScale;

        _currentHP = maxHP;
    }


    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Player")
        {
            HealthManager.Instance.TakeDamage(damage);        
        }

        if (trigger.gameObject.tag == "Weapon")
            TakeDamage();
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
            _particles.Play();
            creature.SetActive(false);
            Destroy(gameObject, 1f);        
        }
    }
}

