using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class InputManager : MonoBehaviour
{

    public List<Text> Keys;
    public List<Binding> KeyBindings;

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
            ActionCallBack = null;
            StopActionCallBack = null;
        }

        public string Action;
        public KeyCode KeyCode;

        public delegate void OnActionRegistered();
        public OnActionRegistered ActionCallBack;
        public OnActionRegistered StopActionCallBack;
    }

    private GameObject CurrentKey;

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
                if (Input.GetKeyDown(binding.KeyCode) && binding.ActionCallBack != null)
                    binding.ActionCallBack.Invoke();

                if (Input.GetKeyUp(binding.KeyCode) && binding.StopActionCallBack != null)
                    binding.StopActionCallBack.Invoke();
            }
        }
    }

    void OnGUI() // for this to be able to work the GameObject CurrentKey(a GUI Button representing the key) needs to have a name that coresponds to a string Name
    {            //on one of the Action objects in the list
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

            if (Code != KeyCode.None && Code != KeyCode.E)
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

                GameManager.GM.InitializeSettings();

                if (GameManager.GM.Player != null) ;
                   //GameManager.GM.Player.RegisterCallbacks();
            }
        }
    }

    public void SetDefaultBindings()
    {
        KeyBindings = new List<Binding>();

        Binding MoveUp = new Binding("Up", KeyCode.UpArrow);
        KeyBindings.Add(MoveUp);

        Binding MoveLeft = new Binding("Left", KeyCode.LeftArrow);
        KeyBindings.Add(MoveLeft);

        Binding MoveDown = new Binding("Down", KeyCode.DownArrow);
        KeyBindings.Add(MoveDown);

        Binding MoveRight = new Binding("Right", KeyCode.RightArrow);
        KeyBindings.Add(MoveRight);

        Binding Attack = new Binding("Attack", KeyCode.Mouse0);
        KeyBindings.Add(Attack);

        Binding Interact = new Binding("Interact", KeyCode.E);
        KeyBindings.Add(Interact);

        /*  Action Inventory = new Action("Inventory", KeyCode.I);
          KeyBindings.Add(Inventory);   */
    }

    public void RefreshDisplayedKeyBindings()
    {
        for (int i = 0; i < KeyBindings.Count; i++)
        {
            Keys[i].text = KeyBindings[i].KeyCode.ToString();
        }
    }

    public void ChangeKey(GameObject clicked) //this is the callback to the OnClickEvent of the Buttons. The gameObject clicked is the Button itself
    {                                                //this is just used to store the button clicked into a variable inside the script
        CurrentKey = clicked;
    }
}
