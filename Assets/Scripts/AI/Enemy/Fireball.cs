using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    private Animator _playerAnim;

    private Transform _transform;
    private Vector3 _scale;

    public float speed;

    void Awake()
    {
        _playerAnim = GameManager.GM.Player.anim;
        _transform = transform;
    }
	
	void Update ()
    {
        if (_transform.localScale.x > 0)
            _transform.position += Vector3.left * speed * Time.deltaTime;
        else
            _transform.position += Vector3.right * speed * Time.deltaTime;      
	}


    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Player")
        {
            if (_playerAnim.GetBool("Blocking") && _playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f && GameManager.GM.Player.transform.localScale.x * transform.localScale.x > 0)
                return;

            HealthManager.Instance.TakeDamage(1);
        }
    }
}
