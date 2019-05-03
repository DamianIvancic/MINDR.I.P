using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour {


    private bool _displayed;

    public enum MessageType
    {
        Berserk1,
        Berserk2,
        Block,
        Dash,
        AirDash,
        AirDamage,
        Run
    }

    public MessageType type;
    private string _message;

    void Update ()
    {
        if (_displayed && Input.GetKeyDown(KeyCode.Return))
            UIManager.Instance.FinishTextDisplay();      
	}

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.gameObject.tag == "Player" && _displayed == false && UIManager.Instance.hintsEnabled)
        {
            List<InputManager.Binding> keyBindings = InputManager.Instance.KeyBindings;

            switch (type)
            {              
                case (MessageType.Berserk1):
                    _message = "Welcome to MINDR.I.P . Keep an eye on your berserk meter - dealing damage will fill it and taking damage will empty it. Also, if you want to quit displaying hints hit escape and go to options.";
                    break;
                case (MessageType.Berserk2):
                    _message = "When your berserk meter is high you'll unlock extra abilities, when it's low the viking will suffer hallucinations and eventually death.";
                    break;
                case (MessageType.Block):                  
                    string attackKey = " ";
                    string blockKey = " ";
                    foreach(InputManager.Binding keyBinding in keyBindings)
                    {
                        if (keyBinding.Action == "Attack")
                            attackKey = keyBinding.KeyCode.ToString();

                        if (keyBinding.Action == "Block")
                            blockKey = keyBinding.KeyCode.ToString();
                    }
                    _message = "Besides attacking with " + attackKey + ", you can also block with " + blockKey + ". Also, check (and adjust if you want to) the keys needed to use your additional berserker abilities under options -> controls.";
                    break;
                case (MessageType.Dash):
                    string dashKey = " ";
                    foreach (InputManager.Binding keyBinding in keyBindings)
                    {                  
                        if (keyBinding.Action == "Dash")
                            dashKey = keyBinding.KeyCode.ToString();
                    }
                    _message = "You can dash with " + dashKey + ".";
                    break;
                case(MessageType.AirDash):
                    _message = "It's also possible to dash in the air.";
                    break;
                case(MessageType.AirDamage):
                    _message = "Watch out for enemies while airborne - if you take damage in the air you'll lose control of your character until you land.";
                    break;
                case(MessageType.Run):
                    string toggleRunKey = " ";
                    foreach (InputManager.Binding keyBinding in keyBindings)
                    {
                        if (keyBinding.Action == "ToggleRun")
                            toggleRunKey = keyBinding.KeyCode.ToString();
                    }
                    _message = "On top of dashing in the air, you can also use " + toggleRunKey + " to toggle run and increase movement speed and jumping distance.";
                    break;
            }
            UIManager.Instance.DisplayText(_message);
            _displayed = true;
        }
    }
}
