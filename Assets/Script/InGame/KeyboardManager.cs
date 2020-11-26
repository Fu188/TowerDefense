using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    [HideInInspector]
    public static KeyboardManager Instance { get; private set; }

    public KeyCode MoveForward;
    public KeyCode MoveBackward;
    public KeyCode MoveLeft;
    public KeyCode MoveRight;
    public KeyCode MoveJump;
    public KeyCode SwitchMode;
    public KeyCode SwitchItem;
    public KeyCode CardModePut;
    public KeyCode CardModeUpgrade;
    public KeyCode CardModeRemove;
    public KeyCode PropModeUse;

    // Start is called before the first frame update
    void Start()
    {
        MoveForward = KeyCode.W;
        MoveBackward = KeyCode.S;
        MoveLeft = KeyCode.A;
        MoveRight = KeyCode.D;
        MoveJump = KeyCode.Space;
        SwitchMode = KeyCode.Tab;
        SwitchItem = KeyCode.Q;
        CardModePut = KeyCode.F;
        CardModeUpgrade = KeyCode.E;
        CardModeRemove = KeyCode.R;
        PropModeUse = KeyCode.F;
    }

    void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
