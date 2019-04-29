using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject PrefabToSpawn;
    public float SpawnInterval;

    private float _spawnTimer;
    private bool _active = false;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(GameManager.GM.CurrentSate == GameManager.GameState.Playing && _active)
        {
            _spawnTimer += Time.deltaTime;
            if(_spawnTimer >= SpawnInterval)
            {
                Instantiate(PrefabToSpawn, transform.position, Quaternion.identity, transform);
                _spawnTimer = 0f;
            }
        }
	}

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.gameObject.tag == "Player")
        {
            _active = true;
            _spawnTimer = SpawnInterval;
        }
    }

    void OnTriggerExit2D(Collider2D trigger)
    {
        if(trigger.gameObject.tag == "Player")
        {
            _active = false;
        }
    }
}
