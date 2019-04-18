using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownManager : MonoBehaviour
{
    public Image StompCDImage;
    public Image FireBreathCDImage;

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
        }
    }

	void Start () {
		
	}

	void Update ()
    {
        if(StompCDActive)
        {
            _stompCDTimer += Time.deltaTime;
            StompCDImage.fillAmount = 1 - _stompCDTimer / _stompCDPeriod;

            if(_stompCDTimer > _stompCDPeriod)
            {
                StompCDActive = false;
                _stompCDTimer = 0f;
            }
        }


		if(FireCDActive)
        {
            _fireCDTimer += Time.deltaTime;
            FireBreathCDImage.fillAmount = 1 - _fireCDTimer / _fireCDPeriod;

            if (_fireCDTimer >= _fireCDPeriod)
            {
                FireCDActive = false;
                _fireCDTimer = 0f;
            }
        }
	}

    public void TriggerStompCD()
    {
        StompCDImage.fillAmount = 1;
        StompCDActive = true;
    }

    public void TriggerFireBreathCD()
    {      
        FireBreathCDImage.fillAmount = 1;
        FireCDActive = true;
    }
}
