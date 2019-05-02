using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {


    private SpriteRenderer _spriteRenderer;
    private Color _color;

    private float _maxHP = 5;
    private float _currentHP;

    private float _maxCharge = 5;
    private float _currentCharge;

    private float _invulnerabilityTimer;
    private bool _alphaIncreasing = false;
    private bool _invulnerable = false;
    
    [HideInInspector]
    public static HealthManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _color = _spriteRenderer.color;

            _currentHP = 3;
            _currentCharge = 0;
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        CooldownManager.Instance.UpdateSkillsActive(_currentHP);

        UIManager.Instance.BerserkMeter.value = _currentHP / 5;
        UIManager.Instance.BerserkChargeFill.fillAmount = _currentCharge / 5;
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
            CooldownManager.Instance.UpdateSkillsActive(_currentHP);
            UIManager.Instance.BerserkMeter.value = _currentHP / 5;
       
            GameManager.GM.Player.damagedSound.Play();

            if (_currentHP <= 0)
                GameManager.GM.RestartScene();

            if (GameManager.GM.Player.isGrounded == false)
                GameManager.GM.Player.controllable = false;

            StartCoroutine(InvulnerabilityEffect());
        }     
    }

    public void ChargeBerserk()
    {
        _currentCharge++;
        if(_currentCharge >= 5)
        {
            if (_currentHP < 5)
            {
                _currentCharge -= 5;
                _currentHP++;
                CooldownManager.Instance.UpdateSkillsActive(_currentHP);
                UIManager.Instance.BerserkMeter.value = _currentHP / 5;
            }
            else
                _currentCharge = 5;
           
        }

        UIManager.Instance.BerserkChargeFill.fillAmount = _currentCharge /5;
    }
}
