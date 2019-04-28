using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CooldownManager : MonoBehaviour
{
    public Image StompCDOverlay;
    public Image FireBreathCDOverlay;

    [HideInInspector]
    public bool StompCDActive = false;
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

            SceneManager.sceneLoaded += OnSceneLoadedListener;
        }
    }

	void Start () {
		
	}

	void Update ()
    {
        if(GameManager.GM.gameState == GameManager.GameState.Playing)
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
