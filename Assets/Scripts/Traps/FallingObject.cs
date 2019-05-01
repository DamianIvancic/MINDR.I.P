using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float waitPeriod;
    private float _timer;

    public float acceleration;
    private float _speed = 0f;

    private bool _fallTriggered;

    void Update()
    {
        if (_fallTriggered)
        {
            transform.position += (Vector3)Random.insideUnitCircle * 0.05f;
            _timer += Time.deltaTime;

            _speed += acceleration;

            if (_timer >= waitPeriod)
            {
                transform.position += new Vector3(0, -1f, 0) * _speed * Time.deltaTime;
                if (_timer > 15)
                    Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            _fallTriggered = true;
        }
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Player")
        {
            _fallTriggered = true;
        }
    }
}
