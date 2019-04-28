using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

    public Text HPText;
    public int maxHP;
    private int _currentHP;

    private SpriteRenderer _spriteRenderer;
    private Color _color;
    private bool _alphaIncreasing = false;
    private bool _invulnerable = false;
    private float _invulnerabilityTimer;

    
    [HideInInspector]
    public static HealthManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _color = _spriteRenderer.color;

            _currentHP = maxHP;
            HPText.text = _currentHP.ToString();
        }
    }

	void Start () {
		
	}
	
	void Update ()
    {  
       
        if (_currentHP <= 0)
            GameManager.GM.RestartScene();
	}

    IEnumerator InvulnerabilityEffect()
    {
        _invulnerabilityTimer = 0;
        _invulnerable = true;

        while (_invulnerabilityTimer < 2)
        {
            _invulnerabilityTimer += Time.deltaTime;

            _color.a += _alphaIncreasing ? 0.02f : -0.02f;

            if (_color.a <= 0.4f || _color.a >= 1f)
                _alphaIncreasing = !_alphaIncreasing;

            _spriteRenderer.color = _color;

            yield return null;
        }
   
        _color.a = 1f;
        _spriteRenderer.color = _color;
        _alphaIncreasing = false;
        _invulnerable = false;
    }

    public void TakeDamage(int damage)
    {
        if(!_invulnerable)
        {        
            _currentHP -= damage;
            GameManager.GM.Player.damagedSound.Play();
            HPText.text = _currentHP.ToString();

            StartCoroutine(InvulnerabilityEffect());
        }     
    }
}
