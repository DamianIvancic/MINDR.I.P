using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


/* Works together with the SLS system and the PlayerController to provide dynamic binding of keys to different actions defined by the PlayerController.
 * By default, this sets a number of bindings to predefined keys, which are then connected to certain callbacks in the PlayerController once that scripts run its Start() .
 * The OnGUI() and the ChangeKey() functions when used together with in-editor UI elements provide ability to dynamically reconfigure bindings at runtime.
 * When bindings are set or re-binded the data is written to a file via the SLS system and on every start-up the InputManager checks if there's a file containing 
 * binding data which it can read from. If yes it loads it in, if no it sets the default ones and makes a file containing that data which is later loaded from and modified when bindings change.*/

public class InputManager : MonoBehaviour
{
     
    public List<Text> Keys;   //list of UI text elements used to display what key is currently bound to the respective action
  
    [System.Serializable]
    public class Binding  
    {
        public Binding(string action, KeyCode key)
        {
            Action = action;
            KeyCode = key;
        }

        public void ClearCallBacks()
        {
            GetKeyDownCallback = null;
            GetKeyCallback = null;
            GetKeyUpCallback = null;
        }

        public string Action;
        public KeyCode KeyCode;

        public delegate void OnActionRegistered();
        public OnActionRegistered GetKeyDownCallback;
        public OnActionRegistered GetKeyCallback;
        public OnActionRegistered GetKeyUpCallback;
    }

    private GameObject CurrentKey; //this gets set by ChangeKey(), which is a callback for the UI Buttons

    [HideInInspector]
    public List<Binding> KeyBindings;
    [HideInInspector]
    public static InputManager Instance;

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            string path = Application.persistentDataPath + "/Settings.dat";

            if (File.Exists(path))
                KeyBindings = SLSManager.Instance.Settings.KeyBindings;
            else
            {
                SetDefaultBindings();
                SLSManager.Instance.SaveSettings();
            }

            RefreshDisplayedKeyBindings();
        }
    }

    void Update()
    {
        if (GameManager.GM.gameState == GameManager.GameState.Playing)
        {

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            foreach (Binding binding in KeyBindings)
            {
                if (Input.GetKeyDown(binding.KeyCode) && binding.GetKeyDownCallback != null)
                    binding.GetKeyDownCallback.Invoke();

                if (Input.GetKey(binding.KeyCode) && binding.GetKeyCallback != null)
                    binding.GetKeyCallback.Invoke();

                if (Input.GetKeyUp(binding.KeyCode) && binding.GetKeyUpCallback != null)
                    binding.GetKeyUpCallback.Invoke();
            }
        }
    }

    void OnGUI() // for this to be able to work the GameObject CurrentKey(a GUI Button representing the key) needs to have a name that coresponds to a string action
    {            //on one of the Binding objects in the list
        if (CurrentKey != null)
        {

            foreach (Binding binding in KeyBindings) //all callbacks need to be cleared whenever a new binding is set because the bindings can't be serialized if the callbacks are registered
            {
                binding.ClearCallBacks();
            }

            Event e = Event.current;
            KeyCode Code = KeyCode.None;

            if (e.isMouse)
                Code = (KeyCode)((int)KeyCode.Mouse0 + e.button);
            else if (e.isKey)
                Code = e.keyCode;

            if (Code != KeyCode.None)
            {
                foreach (Binding binding in KeyBindings) //loops through all bindings 
                {
                    if (binding.KeyCode == Code) //if there's an action already bound to the pressed key unpairs the binding
                    {
                        binding.KeyCode = KeyCode.None;

                        foreach (Text TextKey in Keys)
                        {
                            if (TextKey.text == Code.ToString())
                                TextKey.text = "None";
                        }
                    }

                    if (binding.Action == CurrentKey.name)  //binds the action to the new keycode and updates the text display
                    {
                        binding.KeyCode = Code;
                        CurrentKey.transform.GetChild(0).gameObject.GetComponent<Text>().text = Code.ToString();
                    }
                }

                CurrentKey = null;

                SLSManager.Instance.SaveSettings();

                if (GameManager.GM.Player != null)
                    RegisterCallbacks();
            }
        }
    }

    public void SetDefaultBindings()
    {
        KeyBindings = new List<Binding>();

        Binding MoveRight = new Binding("Right", KeyCode.D);
        KeyBindings.Add(MoveRight);

        Binding MoveLeft = new Binding("Left", KeyCode.A);
        KeyBindings.Add(MoveLeft);

        Binding Jump = new Binding("Jump", KeyCode.Space);
        KeyBindings.Add(Jump);

        Binding Attack = new Binding("Attack", KeyCode.Mouse0);
        KeyBindings.Add(Attack);

        Binding Block = new Binding("Block", KeyCode.Mouse1);
        KeyBindings.Add(Block);

        Binding Dash = new Binding("Dash", KeyCode.W);
        KeyBindings.Add(Dash);

        Binding Stomp = new Binding("Stomp", KeyCode.Q);
        KeyBindings.Add(Stomp);

        Binding FireBreath = new Binding("FireBreath", KeyCode.Mouse2);
        KeyBindings.Add(FireBreath);

        Binding ToggleRun = new Binding("ToggleRun", KeyCode.LeftShift);
        KeyBindings.Add(ToggleRun);
    }

    public void RefreshDisplayedKeyBindings()
    {
        for (int i = 0; i < KeyBindings.Count; i++)
        {
            Keys[i].text = KeyBindings[i].KeyCode.ToString().ToUpper();
        }
    }

    public void ChangeKey(GameObject clicked) //this is the callback to the OnClickEvent of the Buttons. The gameObject clicked is the Button itself
    {                                                //this is just used to store the button clicked into a variable inside the script
        CurrentKey = clicked;
    }

    public void RegisterCallbacks()
    {
        if (GameManager.GM.Player == null)
            Debug.Log("GM.Player not registered!");
        else
        {
            foreach (Binding binding in KeyBindings)
            {
                switch (binding.Action)
                {

                    case ("Right"):
                        binding.GetKeyCallback += GameManager.GM.Player.MoveRight;
                        binding.GetKeyUpCallback += GameManager.GM.Player.StopMovingHorizontal;
                        break;
                    case ("Left"):
                        binding.GetKeyCallback += GameManager.GM.Player.MoveLeft;
                        binding.GetKeyUpCallback += GameManager.GM.Player.StopMovingHorizontal;
                        break;
                    case ("Jump"):
                        binding.GetKeyDownCallback += GameManager.GM.Player.Jump;
                        binding.GetKeyUpCallback += GameManager.GM.Player.StopJump;
                        break;
                    case ("Attack"):
                        binding.GetKeyDownCallback += GameManager.GM.Player.Attack;
                        break;
                    case ("Dash"):
                        binding.GetKeyDownCallback += GameManager.GM.Player.Dash;
                        break;
                    case ("Stomp"):
                        binding.GetKeyDownCallback += GameManager.GM.Player.Stomp;
                        break;
                    case ("Block"):
                        binding.GetKeyCallback += GameManager.GM.Player.Block;
                        binding.GetKeyUpCallback += GameManager.GM.Player.StopBlock;
                        break;
                    case ("FireBreath"):
                        binding.GetKeyDownCallback += GameManager.GM.Player.FireBreath;
                        binding.GetKeyUpCallback += GameManager.GM.Player.StopFireBreath;
                        break;
                    case ("ToggleRun"):
                        binding.GetKeyDownCallback += GameManager.GM.Player.ToggleRun;
                        break;
                }
            }
        }  
    }

    public void ClearCallbacks()
    {
        if(KeyBindings != null)
        {
            foreach(Binding binding in KeyBindings)
            {
                binding.ClearCallBacks();
            }
        }
    }
}
