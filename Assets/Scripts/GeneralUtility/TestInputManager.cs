using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestInputManager : MonoBehaviour
{

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

    [HideInInspector]
    public List<Binding> KeyBindings;
    [HideInInspector]
    public static TestInputManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            SetDefaultBindings();            
        }
    }


    void Update()
    {
        if (GameManager.GM.CurrentSate == GameManager.GameState.Playing)
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
        if (KeyBindings != null)
        {
            foreach (Binding binding in KeyBindings)
            {
                binding.ClearCallBacks();
            }
        }
    }
}
