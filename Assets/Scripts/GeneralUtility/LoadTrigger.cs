using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTrigger : MonoBehaviour
{
    public enum TriggerType
    {
        Death,
        LevelCompleted
    }

    public TriggerType type;

    void OnTriggerEnter2D(Collider2D trigger)
    {
        switch (type)
        {
            case (TriggerType.Death):
                GameManager.GM.RestartScene();
                break;
            case (TriggerType.LevelCompleted):
                GameManager.GM.LoadNextScene();
                break;
        }
    }
}
