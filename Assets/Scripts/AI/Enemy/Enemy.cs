using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    private AudioSource _damagedSound;
    private ParticleSystem _particles;

    private Transform _transform;
    private Transform _parent;

    public int maxHP;
    public int damage;
    private int _currentHP;

    [HideInInspector]
    public bool aggro;

    protected virtual void Awake()
    {
        _damagedSound = transform.parent.gameObject.GetComponent<AudioSource>();
        _particles = transform.parent.gameObject.GetComponent<ParticleSystem>();

        _transform = transform;
        _parent = transform.parent;

        _currentHP = maxHP;
    }

    protected virtual void Update()
    {
        _parent.position = transform.position;
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

	public void SetAggro(bool state)
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
            Destroy(gameObject.transform.parent.gameObject, 1f);
            gameObject.SetActive(false);
        }
    }
}

