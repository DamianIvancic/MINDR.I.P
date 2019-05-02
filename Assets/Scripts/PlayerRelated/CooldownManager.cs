using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CooldownManager : MonoBehaviour
{

    public Image StompIcon;
    public Image FireIcon;

    public Image StompCDOverlay;
    public Image FireBreathCDOverlay;

    [HideInInspector]
    public bool StompSkillActive = false; //a skill is active if the berserk meter is high enough
    [HideInInspector]
    public bool FireSkillActive = false;
    [HideInInspector]
    public bool StompCDActive = false;  //this controls whether the skill is on cooldown
    [HideInInspector]
    public bool FireCDActive = false;

    private float _stompCDPeriod = 7.5f;
    private float _stompCDTimer = 0f;
    private float _fireCDPeriod = 15f;
    private float _fireCDTimer = 0f;

    public static CooldownManager Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            StompIcon.gameObject.SetActive(StompSkillActive);
            FireIcon.gameObject.SetActive(FireSkillActive);

            SceneManager.sceneLoaded += OnSceneLoadedListener;
        }
    }

	void Update ()
    {
        if(GameManager.GM.CurrentSate == GameManager.GameState.Playing)
        {
            if (StompCDActive)
            {
                _stompCDTimer += Time.deltaTime;
                StompCDOverlay.fillAmount = 1 - _stompCDTimer / _stompCDPeriod;

                if (_stompCDTimer > _stompCDPeriod)
                {
                    StompCDActive = false;
                    _stompCDTimer = 0f;
                }
            }


            if (FireCDActive)
            {
                _fireCDTimer += Time.deltaTime;
                FireBreathCDOverlay.fillAmount = 1 - _fireCDTimer / _fireCDPeriod;

                if (_fireCDTimer >= _fireCDPeriod)
                {
                    FireCDActive = false;
                    _fireCDTimer = 0f;
                }
            }
        }     
	}

    void OnSceneLoadedListener(Scene scene, LoadSceneMode mode)
    {
        _stompCDTimer = 0f;
        _fireCDTimer = 0f;

        StompCDOverlay.fillAmount = 0;
        FireBreathCDOverlay.fillAmount = 0;

        StompCDActive = false;
        FireCDActive = false;
    }

    public void UpdateSkillsActive(float currentHP)
    {
        if (currentHP >= 4)
        {
            StompSkillActive = true;
            if (currentHP == 5)
                FireSkillActive = true;
            else
                FireSkillActive = false;
        }
        else
        {
            StompSkillActive = false;
            FireSkillActive = false;
        }

        StompIcon.gameObject.SetActive(StompSkillActive);
        FireIcon.gameObject.SetActive(FireSkillActive);
    }

    public void TriggerStompCD()
    {
        StompCDOverlay.fillAmount = 1;
        StompCDActive = true;
    }

    public void TriggerFireBreathCD()
    {      
        FireBreathCDOverlay.fillAmount = 1;
        FireCDActive = true;
    }
}
