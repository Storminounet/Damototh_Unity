using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerType
{
    Null = 0,
    Keyboard = 1,
    PS4 = 2,
    xBox = 3
}

public enum ControllerButton
{
    RightPadDown,
    RightPadRight,
    RightPadUp,
    RightPadLeft,
    LeftPadDown,
    LeftPadRight,
    LeftPadUp,
    LeftPadLeft,
    RightTrigger,
    RightBumper,
    RightMenu,
    RightClick,
    LeftClick,
    LeftMenu,
    LeftBumper,
    LeftTrigger,
    PS,
    Pad
}
public enum ControllerAxis
{
    leftStickHorizontal = 0,
    leftStickVertical = 1,
    rightStickHorizontal = 2,
    rightStickVertical = 3,
    LeftTrigger = 4,
    RightTrigger = 5,
    padHorizontal = 6,
    padVertical = 7
}

public enum ComputerInputType
{
    Mouse = 0,
    Keyboard = 1,
    Wheel = 2
}

[System.Serializable]
public class ComputerInput
{
    public enum WheelDirectionEnum
    {
        Up,
        Down
    }

    [SerializeField] private ComputerInputType _inputType;
    [SerializeField] private InputManager.InputMode _inputMode;
    [SerializeField] private KeyCode _key;
    [SerializeField] private int _mouseButtonId;
    [SerializeField] private WheelDirectionEnum _wheelDirection;

    public ComputerInputType InputType { get { return _inputType; } }
    public KeyCode Key { get { return _key; } }
    public int MouseButtonId { get { return _mouseButtonId; } }
    public WheelDirectionEnum WheelDirection { get { return _wheelDirection; } }

#if UNITY_EDITOR
    public bool Opened;
#endif

    public bool IsPressed()
    {
        switch (_inputType)
        {
            case ComputerInputType.Mouse:
                switch (_inputMode)
                {
                    case InputManager.InputMode.Held:
                        return Input.GetMouseButton(_mouseButtonId);
                    case InputManager.InputMode.Down:
                        return Input.GetMouseButtonDown(_mouseButtonId);
                    case InputManager.InputMode.Up:
                        return Input.GetMouseButtonUp(_mouseButtonId);
                }
                break;
            case ComputerInputType.Keyboard:
                switch (_inputMode)
                {
                    case InputManager.InputMode.Held:
                        return Input.GetKey(_key);
                    case InputManager.InputMode.Down:
                        return Input.GetKeyDown(_key);
                    case InputManager.InputMode.Up:
                        return Input.GetKeyUp(_key);
                }
                break;
            case ComputerInputType.Wheel:
                bool cur = false, last = false;
                switch (_wheelDirection)
                {
                    case WheelDirectionEnum.Up:
                        cur = InputManager.WheelState == InputManager.InputState.Positive;
                        last = InputManager.LastWheelState == InputManager.InputState.Positive;
                        break;
                    case WheelDirectionEnum.Down:
                        cur = InputManager.WheelState == InputManager.InputState.Negative;
                        last = InputManager.LastWheelState == InputManager.InputState.Negative;
                        break;
                }
                switch (_inputMode)
                {
                    case InputManager.InputMode.Held:
                        return cur == true && last == true;
                    case InputManager.InputMode.Down:
                        return cur == true && last == false;
                    case InputManager.InputMode.Up:
                        return cur == false && last == true;
                }
                break;
        }

        return false;
    }
}

public class InputManager : Singleton<InputManager>
{
    public enum StickSide
    {
        Left,
        Right
    }
    public enum InputMode
    {
        Held = 0,
        Down = 1,
        Up = 2
    }
    public enum InputState
    {
        None,
        Positive,
        Negative
    }
    
    private static string[] _controllerNames;
    private static ControllerType[] _controllerTypes;

    private static List<InputState[]> _lastAxisInput;
    private static List<InputState[]> _axisInput;

    #region Inputs
    public static bool InputingMovement { get { return _inputingMovement; } }
    public static bool InputingLook { get { return _inputingLook; } }

    public static bool Sprint { get { return _sprint; } }
    public static bool Dodge { get { return _dodge; } }

    public static bool AutoRotate { get { return _autoRotate; } }
    public static bool LockDown { get { return _lockDown; } }
    public static bool LockUp { get { return _lockUp; } }
    public static bool LockLeftTarget { get { return _lockLeftTarget; } }
    public static bool LockRightTarget { get { return _lockRightTarget; } }

    public static bool LightAttack { get { return _lightAttack; } }
    public static bool HeavyAttack { get { return _heavyAttack; } }
    public static bool HydraAttackOne { get { return _hydraAttackOne; } }
    public static bool HydraAttackTwo { get { return _hydraAttackTwo; } }
    #endregion


    public static float MoveInputMagntiude { get { return _moveInputMagnitude; } }
    public static float LookInputMagntiude { get { return _lookInputMagnitude; } }

    public static Vector2 MoveInput { get { return _moveInput; } }
    public static Vector2 MoveInputNormalized { get { return _moveInput; } }
    public static Vector2 LookInput { get { return _lookInput; } }
    public static Vector2 LookInputNormalized { get { return _lookInput; } }

    public static InputState WheelState { get { return _wheelState; } }
    public static InputState LastWheelState { get { return _lastWheelState; } }

    //Instance
    [SerializeField] private P_InputData _iData;

    private static bool _inputingMovement;
    private static bool _inputingLook;

    private static bool _sprint;
    private static bool _dodge;

    private static bool _autoRotate;
    private static bool _lockDown;
    private static bool _lockUp;
    private static bool _lockLeftTarget;
    private static bool _lockRightTarget;

    private static bool _lightAttack;
    private static bool _heavyAttack;
    private static bool _hydraAttackOne;
    private static bool _hydraAttackTwo;


    private static float _wheelAxis;
    private static InputState _wheelState;
    private static InputState _lastWheelState;

    private static float _moveInputMagnitude;
    private static float _lookInputMagnitude;

    private static Vector2 _moveInput;
    private static Vector2 _moveInputNormalized;
    private static Vector2 _lookInput;
    private static Vector2 _lookInputNormalized;

    private void Awake()
    {
        SetInstance(this);
        _lastAxisInput = new List<InputState[]>();
        _axisInput = new List<InputState[]>();

        _controllerNames = new string[0];
        _controllerTypes = new ControllerType[0];
        ComputeControllerTypes();
    }

    private void Update()
    {
        CheckNewControllers();
        ComputeAxisToButton();
        ComputeMoveInput();
        ComputeLookInput();
        ComputeActionsInputs();
    }

    private void CheckNewControllers()
    {
        if (_controllerNames.Length < Input.GetJoystickNames().Length)
        {
            ComputeControllerTypes();
        }
    }

    private void ComputeAxisToButton()
    {
        //Wheel
        _lastWheelState = _wheelState;
        _wheelAxis = Input.GetAxis("Mouse ScrollWheel");
        _wheelState = (_wheelAxis > 0.8f ? InputState.Positive : (_wheelAxis < -0.8f ? InputState.Negative : InputState.None));

        //Controllers
        StoreLastAxisInput();
        _axisInput.Clear();

        float axisValue;
        for (int controllerId = 1; controllerId <= _controllerTypes.Length; controllerId++)
        {
            _axisInput.Add(new InputState[8]);

            for (int axisId = 0; axisId < 8; axisId++)
            {
                axisValue = GetControllerAxis((ControllerAxis)axisId, controllerId, _controllerTypes[controllerId - 1]);
                _axisInput[controllerId - 1][axisId] = 
                    (axisValue > 0.8f ? InputState.Positive :
                    (axisValue < -0.8f ? InputState.Negative :
                    InputState.None));
            }
        }


    }
    private void StoreLastAxisInput()
    {
        _lastAxisInput.Clear();
        for (int i = 0; i < _axisInput.Count; i++)
        {
            _lastAxisInput.Add(_axisInput[i]);
        }
    }

    private void ComputeMoveInput()
    {
        switch (_iData.ControllerType)
        {
            case ControllerType.Keyboard:
                _moveInput.x = 0;
                _moveInput.y = 0;

                if (Input.GetKey(_iData.Shortcuts.K_MoveForward))
                {
                    _moveInput.y++;
                }
                if (Input.GetKey(_iData.Shortcuts.K_MoveBackward))
                {
                    _moveInput.y--;
                }
                if (Input.GetKey(_iData.Shortcuts.K_MoveRight))
                {
                    _moveInput.x++;
                }
                if (Input.GetKey(_iData.Shortcuts.K_MoveLeft))
                {
                    _moveInput.x--;
                }

                break;
            default:
                _moveInput.x = GetControllerAxis(_iData.Shortcuts.C_MoveHorizontalAxis, _iData.ControllerId, _iData.ControllerType);
                _moveInput.y = GetControllerAxis(_iData.Shortcuts.C_MoveVerticalAxis, _iData.ControllerId, _iData.ControllerType);
                _moveInput = ComputeControllerSensitivity(_moveInput, _iData.ControllerType, StickSide.Left);
                break;
        }

        if (_moveInput.x == 0 && _moveInput.y == 0)
        {
            _inputingMovement = false;
            _moveInputMagnitude = 0;
            _moveInputNormalized = _moveInput * 0;
        }
        else
        {
            _inputingMovement = true;

            _moveInputMagnitude = _moveInput.magnitude;
            _moveInputNormalized = _moveInput / _moveInputMagnitude;

            if (_moveInputMagnitude > 1)
            {
                _moveInputMagnitude = 1;
                _moveInput = _moveInputNormalized;
            }
        }
    }

    private void ComputeLookInput()
    {
        switch (_iData.ControllerType)
        {
            case ControllerType.Keyboard:
                _lookInput.x = Input.GetAxis("Mouse X");
                _lookInput.y = Input.GetAxis("Mouse Y");
                _lookInput *= _iData.MouseSensitivity;
                break;
            default:
                _lookInput.x = GetControllerAxis(ControllerAxis.rightStickHorizontal, _iData.ControllerId, _iData.ControllerType);
                _lookInput.y = GetControllerAxis(ControllerAxis.rightStickVertical, _iData.ControllerId, _iData.ControllerType);
                _lookInput = ComputeControllerSensitivity(_lookInput, _iData.ControllerType, StickSide.Right);
                break;
        }

        if (_lookInput.x == 0 && _lookInput.y == 0)
        {
            _inputingLook = false;
            _lookInputMagnitude = 0;
            _lookInputNormalized = _lookInput * 0;
        }
        else
        {
            _inputingLook = true;

            _lookInputMagnitude = _lookInput.magnitude;
            _lookInputNormalized = _lookInput / _lookInputMagnitude;
        }
    }

    private void ComputeActionsInputs()
    {
        switch (_iData.ControllerType)
        {
            case ControllerType.Keyboard:
                _sprint = Input.GetKeyDown(_iData.Shortcuts.K_Sprint);
                _dodge = Input.GetKeyDown(_iData.Shortcuts.K_Dodge);

                _autoRotate = Input.GetKeyDown(_iData.Shortcuts.K_AutoRotate);
                _lockUp = Input.GetKeyUp(_iData.Shortcuts.K_Lock);
                _lockDown = Input.GetKeyDown(_iData.Shortcuts.K_Lock);

                _lockLeftTarget = _lastWheelState == InputState.None && _wheelState == InputState.Positive;
                _lockRightTarget = _lastWheelState == InputState.None && _wheelState == InputState.Negative;

                _lightAttack = _iData.Shortcuts.K_LightAttack.IsPressed();
                _heavyAttack = _iData.Shortcuts.K_HeavyAttack.IsPressed();
                _hydraAttackOne = _iData.Shortcuts.K_HydraAttackOne.IsPressed();
                _hydraAttackTwo = _iData.Shortcuts.K_HydraAttackTwo.IsPressed();
                break;
            default:
                _sprint = GetControllerButton(_iData.Shortcuts.C_Sprint, _iData.ControllerId, InputMode.Down);
                _dodge = GetControllerButton(_iData.Shortcuts.C_Dodge, _iData.ControllerId, InputMode.Down);

                _autoRotate = GetControllerButton(_iData.Shortcuts.C_AutoRotate, _iData.ControllerId, InputMode.Down);
                _lockUp = GetControllerButton(_iData.Shortcuts.C_Lock, _iData.ControllerId, InputMode.Up);
                _lockDown = GetControllerButton(_iData.Shortcuts.C_Lock, _iData.ControllerId, InputMode.Down);

                _lockLeftTarget = GetControllerAxis(ControllerAxis.rightStickHorizontal, _iData.ControllerId, InputMode.Down, false);
                _lockRightTarget = GetControllerAxis(ControllerAxis.rightStickHorizontal, _iData.ControllerId, InputMode.Down, true);

                _lightAttack = GetControllerButton(_iData.Shortcuts.C_LightAttack, _iData.ControllerId, InputMode.Down);
                _heavyAttack = GetControllerButton(_iData.Shortcuts.C_HeavyAttack, _iData.ControllerId, InputMode.Down);
                _hydraAttackOne = GetControllerButton(_iData.Shortcuts.C_HydraAttackOne, _iData.ControllerId, InputMode.Down);
                _hydraAttackTwo = GetControllerButton(_iData.Shortcuts.C_HydraAttackTwo, _iData.ControllerId, InputMode.Down);

                break;
        }
    }

    private void ComputeControllerTypes()
    {
        _controllerNames = Input.GetJoystickNames();
        _controllerTypes = new ControllerType[_controllerNames.Length];

        for (int i = 0; i < _controllerNames.Length; i++)
        {
            _controllerTypes[i] = ComputeControllerType(i + 1);
        }
    }


    //Utilities
    public Vector2 ComputeControllerSensitivity(Vector2 rawSensitivity, ControllerType controllerType, StickSide side)
    {
        if (controllerType == ControllerType.Keyboard)
        {
            return rawSensitivity;
        }

        if (controllerType == ControllerType.PS4)
        {
            switch (side)
            {
                case StickSide.Left:
                    return Utilities.DeadzoneVector(rawSensitivity,
                        _iData.PS4Sensitivity.LeftStick.InnerDeadZone,
                        _iData.PS4Sensitivity.LeftStick.OutDeadZone,
                        _iData.PS4Sensitivity.LeftStick.Linearity);
                case StickSide.Right:
                    rawSensitivity = Utilities.DeadzoneVector(rawSensitivity,
                        _iData.PS4Sensitivity.RightStick.InnerDeadZone,
                        _iData.PS4Sensitivity.RightStick.OutDeadZone,
                        _iData.PS4Sensitivity.RightStick.Linearity);
                    rawSensitivity.x *= _iData.PS4Sensitivity.RightStick.HSensitivity;
                    rawSensitivity.y *= _iData.PS4Sensitivity.RightStick.VSensitivity;
                    return rawSensitivity;
            }
        }
        else
        {
            switch (side)
            {
                case StickSide.Left:
                    return Utilities.DeadzoneVector(rawSensitivity,
                        _iData.XBoxSensitivity.LeftStick.InnerDeadZone,
                        _iData.XBoxSensitivity.LeftStick.OutDeadZone,
                        _iData.XBoxSensitivity.LeftStick.Linearity);
                case StickSide.Right:
                    rawSensitivity = Utilities.DeadzoneVector(rawSensitivity,
                        _iData.XBoxSensitivity.RightStick.InnerDeadZone,
                        _iData.XBoxSensitivity.RightStick.OutDeadZone,
                        _iData.XBoxSensitivity.RightStick.Linearity);
                    rawSensitivity.x *= _iData.XBoxSensitivity.RightStick.HSensitivity;
                    rawSensitivity.y *= _iData.XBoxSensitivity.RightStick.VSensitivity;
                    return rawSensitivity;
            }
        }

        throw new System.NotImplementedException();
    }


    private static ControllerType ComputeControllerType(int id)
    {
        if (id == 0)
        {
            return ControllerType.Keyboard;
        }

        string[] joystickNames = Input.GetJoystickNames();

        switch (joystickNames[id - 1].Length)
        {
            case 19:
                return ControllerType.PS4;
            case 33:
                return ControllerType.xBox;
        }

        return ControllerType.xBox;
    }

    private static ControllerType GetControllerType(int id)
    {
        if (id == 0)
        {
            return ControllerType.Keyboard;
        }

        if (id > _controllerTypes.Length)
        {
            return ControllerType.Null;
        }

        return _controllerTypes[id - 1];
    }

    delegate bool GetKey(KeyCode key);
    delegate bool GetMouseButton(int button);
    public static bool GetControllerButton(ControllerButton button, int id, InputMode inputMode = InputMode.Down, ControllerType type = ControllerType.Null)
    {
        GetKey getKey;
        if (inputMode == InputMode.Held)
            getKey = Input.GetKey;
        else if (inputMode == InputMode.Up)
            getKey = Input.GetKeyUp;
        else
            getKey = Input.GetKeyDown;

        switch (type == ControllerType.Null ? GetControllerType(id) : type)
        {
            case ControllerType.PS4:
                switch (button)
                {
                    case ControllerButton.RightPadDown:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button1"));
                    case ControllerButton.RightPadRight:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button2"));
                    case ControllerButton.RightPadUp:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button3"));
                    case ControllerButton.RightPadLeft:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button0"));
                    case ControllerButton.RightTrigger:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button7"));
                    case ControllerButton.RightBumper:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button5"));
                    case ControllerButton.RightMenu:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button9"));
                    case ControllerButton.RightClick:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button11"));
                    case ControllerButton.LeftClick:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button10"));
                    case ControllerButton.LeftMenu:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button8"));
                    case ControllerButton.LeftBumper:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button4"));
                    case ControllerButton.LeftTrigger:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button6"));
                    case ControllerButton.PS:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button12"));
                    case ControllerButton.Pad:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button13"));

                }
                break;
            case ControllerType.xBox:
                switch (button)
                {
                    case ControllerButton.RightPadDown:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button0"));
                    case ControllerButton.RightPadRight:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button1"));
                    case ControllerButton.RightPadUp:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button3"));
                    case ControllerButton.RightPadLeft:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button2"));
                    case ControllerButton.RightBumper:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button5"));
                    case ControllerButton.RightMenu:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button7"));
                    case ControllerButton.RightClick:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button9"));
                    case ControllerButton.LeftClick:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button8"));
                    case ControllerButton.LeftMenu:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button6"));
                    case ControllerButton.LeftBumper:
                        return getKey((KeyCode)System.Enum.Parse(typeof(KeyCode), "Joystick" + id + "Button4"));
                    case ControllerButton.RightTrigger:
                        return GetControllerAxis(ControllerAxis.RightTrigger, id, inputMode);
                    case ControllerButton.LeftTrigger:
                        return GetControllerAxis(ControllerAxis.LeftTrigger, id, inputMode);
                    case ControllerButton.PS:
                        return false;
                    case ControllerButton.Pad:
                        return false;
                }
                break;

        }
        switch (button)
        {
            case ControllerButton.LeftPadDown:
                return GetControllerAxis(ControllerAxis.padVertical, id, inputMode, false);
            case ControllerButton.LeftPadUp:
                return GetControllerAxis(ControllerAxis.padVertical, id, inputMode, true);
            case ControllerButton.LeftPadLeft:
                return GetControllerAxis(ControllerAxis.padHorizontal, id, inputMode, false);
            case ControllerButton.LeftPadRight:
                return GetControllerAxis(ControllerAxis.padHorizontal, id, inputMode, true);
        }

        return false;
    }

    public static float GetControllerAxis(ControllerAxis axis, int id, ControllerType type = ControllerType.Null)
    {
        switch (type == ControllerType.Null ? GetControllerType(id) : type)
        {
            case ControllerType.PS4:
                switch (axis)
                {
                    case ControllerAxis.leftStickHorizontal:
                        return Input.GetAxis("Joystick" + id + "AxisX");
                    case ControllerAxis.leftStickVertical:
                        return Input.GetAxis("Joystick" + id + "AxisY") * -1;
                    case ControllerAxis.rightStickHorizontal:
                        return Input.GetAxis("Joystick" + id + "Axis3");
                    case ControllerAxis.rightStickVertical:
                        return Input.GetAxis("Joystick" + id + "Axis6") * -1;
                    case ControllerAxis.LeftTrigger:
                        return Input.GetAxis("Joystick" + id + "Axis4");
                    case ControllerAxis.RightTrigger:
                        return Input.GetAxis("Joystick" + id + "Axis5");
                    case ControllerAxis.padHorizontal:
                        return Input.GetAxis("Joystick" + id + "Axis7");
                    case ControllerAxis.padVertical:
                        return Input.GetAxis("Joystick" + id + "Axis8");
                    default:
                        break;
                }
                break;
            case ControllerType.xBox:
                switch (axis)
                {
                    case ControllerAxis.leftStickHorizontal:
                        return Input.GetAxis("Joystick" + id + "AxisX");
                    case ControllerAxis.leftStickVertical:
                        return Input.GetAxis("Joystick" + id + "AxisY") * -1;
                    case ControllerAxis.rightStickHorizontal:
                        return Input.GetAxis("Joystick" + id + "Axis4");
                    case ControllerAxis.rightStickVertical:
                        return Input.GetAxis("Joystick" + id + "Axis5") * -1;
                    case ControllerAxis.LeftTrigger:
                        return (Input.GetAxis("Joystick" + id + "Axis9") - 0.5f) * 2f;
                    case ControllerAxis.RightTrigger:
                        return (Input.GetAxis("Joystick" + id + "Axis10") - 0.5f) * 2f;
                    case ControllerAxis.padHorizontal:
                        return Input.GetAxis("Joystick" + id + "Axis6");
                    case ControllerAxis.padVertical:
                        return Input.GetAxis("Joystick" + id + "Axis7");
                    default:
                        break;
                }
                break;
        }
        return 0f;
    }
    public static bool GetControllerAxis(ControllerAxis axis, int id, InputMode InputMode = InputMode.Held, bool positiveComparison = true)
    {
        if (_lastAxisInput.Count == 0)
        {
            return false;
        }

        if (positiveComparison)
        {
            switch (InputMode)
            {
                case InputMode.Held://held
                    return _axisInput[id - 1][(int)axis] == InputState.Positive;
                case InputMode.Down://down
                    return _axisInput[id - 1][(int)axis] == InputState.Positive && _lastAxisInput[id - 1][(int)axis] != InputState.Positive;
                case InputMode.Up://u
                    return _axisInput[id - 1][(int)axis] != InputState.Positive && _lastAxisInput[id - 1][(int)axis] == InputState.Positive;
            }
        }
        else
        {
            switch (InputMode)
            {
                case InputMode.Held://held
                    return _axisInput[id - 1][(int)axis] == InputState.Negative;
                case InputMode.Down://down
                    return _axisInput[id - 1][(int)axis] == InputState.Negative && _lastAxisInput[id - 1][(int)axis] != InputState.Negative;
                case InputMode.Up://up
                    return _axisInput[id - 1][(int)axis] != InputState.Negative && _lastAxisInput[id - 1][(int)axis] == InputState.Negative;
            }
        }


        throw new System.NotImplementedException();
    }
}
