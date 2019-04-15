using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Settings 
{
    public List<InputManager.Binding> KeyBindings;

    public Settings(List<InputManager.Binding> keyBindings)
    {
        KeyBindings = keyBindings;
    }
}
