using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif



[CreateAssetMenu(fileName = "InputData", menuName = "PlayerData/InputData", order = 1000)]
public class P_InputData : ScriptableObject 
{
    [System.Serializable]
    public class ShortcutData
    {
        [Header("Keyboard")]
        [SerializeField] private ComputerInput k_MoveForward;
        [SerializeField] private ComputerInput k_MoveBackward;
        [SerializeField] private ComputerInput k_MoveRight;
        [SerializeField] private ComputerInput k_MoveLeft;
        [Space]
        [SerializeField] private ComputerInput k_Sprint;
        [SerializeField] private ComputerInput k_Dodge;
        [Space]
        [SerializeField] private ComputerInput k_AutoRotate;
        [SerializeField] private ComputerInput k_LockUp;
        [SerializeField] private ComputerInput k_LockDown;
        [SerializeField] private ComputerInput k_LockLeft;
        [SerializeField] private ComputerInput k_LockRight;
        [Space]
        [SerializeField] private ComputerInput k_LightAttack;
        [SerializeField] private ComputerInput k_HeavyAttack;
        [SerializeField] private ComputerInput k_HydraAttackOne;
        [SerializeField] private ComputerInput k_HydraAttackTwo;
        [Space]
        [SerializeField] private ComputerInput k_Interact;


        [Header("Controller")]
        [SerializeField] private ControllerAxis c_MoveVerticalAxis;
        [SerializeField] private ControllerAxis c_MoveHorizontalAxis;
        [Space]
        [SerializeField] private ControllerButton c_Sprint;
        [SerializeField] private ControllerButton c_Dodge;
        [SerializeField] private ControllerButton c_AutoRotate;
        [SerializeField] private ControllerButton c_Lock;
        [Space]
        [SerializeField] private ControllerButton c_LightAttack;
        [SerializeField] private ControllerButton c_HeavyAttack;
        [SerializeField] private ControllerButton c_HydraAttackOne;
        [SerializeField] private ControllerButton c_HydraAttackTwo;
        [Space]
        [SerializeField] private ControllerButton c_Interact;

        //Keyboard
        public ComputerInput K_MoveForward { get { return k_MoveForward; } }
        public ComputerInput K_MoveBackward { get { return k_MoveBackward; } }
        public ComputerInput K_MoveRight { get { return k_MoveRight; } }
        public ComputerInput K_MoveLeft { get { return k_MoveLeft; } }

        public ComputerInput K_Sprint { get { return k_Sprint; } }
        public ComputerInput K_Dodge { get { return k_Dodge; } }

        public ComputerInput K_AutoRotate { get { return k_AutoRotate; } }
        public ComputerInput K_LockUp { get { return k_LockUp; } }
        public ComputerInput K_LockDown { get { return k_LockDown; } }
        public ComputerInput K_LockLeft { get { return k_LockLeft; } }
        public ComputerInput K_LockRight { get { return k_LockRight; } }

        public ComputerInput K_LightAttack { get { return k_LightAttack; } }
        public ComputerInput K_HeavyAttack { get { return k_HeavyAttack; } }
        public ComputerInput K_HydraAttackOne { get { return k_HydraAttackOne; } }
        public ComputerInput K_HydraAttackTwo { get { return k_HydraAttackTwo; } }

        public ComputerInput K_Interact { get { return k_Interact; } }

        //Controller
        public ControllerAxis C_MoveVerticalAxis { get { return c_MoveVerticalAxis; } }
        public ControllerAxis C_MoveHorizontalAxis { get { return c_MoveHorizontalAxis; } }

        public ControllerButton C_Sprint { get { return c_Sprint; } }
        public ControllerButton C_Dodge { get { return c_Dodge; } }
        public ControllerButton C_AutoRotate { get { return c_AutoRotate; } }
        public ControllerButton C_Lock { get { return c_Lock; } }

        public ControllerButton C_LightAttack { get { return c_LightAttack; } }
        public ControllerButton C_HeavyAttack { get { return c_HeavyAttack; } }
        public ControllerButton C_HydraAttackOne { get { return c_HydraAttackOne; } }
        public ControllerButton C_HydraAttackTwo { get { return c_HydraAttackTwo; } }

        public ControllerButton C_Interact { get { return c_Interact; } }

    }

    [System.Serializable]
    public class ControllerSensitivityData
    {
        [System.Serializable]
        public class SensitivityData
        {
            [SerializeField] private float _innerDeadZone = 0.1f;
            [SerializeField] private float _outDeadZone = 0.1f;
            [SerializeField] private float _linearity = 1.3f;

            public float InnerDeadZone { get { return _innerDeadZone; } }
            public float OutDeadZone { get { return _outDeadZone; } }
            public float Linearity { get { return _linearity; } }
        }

        [System.Serializable]
        public class SensitivityScalableData : SensitivityData
        {
            [SerializeField] private float _hSensitivity = 2f;
            [SerializeField] private float _vSensitivity = 1.5f;

            public float HSensitivity { get { return _hSensitivity; } }
            public float VSensitivity { get { return _vSensitivity; } }
        }

        [SerializeField] private SensitivityData _leftStick;
        [SerializeField] private SensitivityScalableData _rightStick;

        public SensitivityData LeftStick { get { return _leftStick; } }
        public SensitivityScalableData RightStick { get { return _rightStick; } }

    }
    [Header("Controller")]
    [Space]
    [SerializeField] private ControllerType _controllerType;
    [SerializeField] private int _controllerId;

    [Header("Keyboard Shortcuts")]
    [Space]
    [SerializeField] private ShortcutData _shortcuts;

    [Header("Controllers & Mouse Sensitivity")]
    [Space]
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private ControllerSensitivityData _ps4Sensitivity;
    [SerializeField] private ControllerSensitivityData _xBoxSensitivity;

    public ControllerType ControllerType { get { return _controllerType; } }
    public int ControllerId { get { return _controllerId; } }
    public ShortcutData Shortcuts { get { return _shortcuts; } }

    public float MouseSensitivity { get { return _mouseSensitivity; } }
    public ControllerSensitivityData PS4Sensitivity { get { return _ps4Sensitivity; } }
    public ControllerSensitivityData XBoxSensitivity { get { return _xBoxSensitivity; } }



    public void SetControllerType(ControllerType type, int id = 0)
    {
        _controllerType = type;
        _controllerId = id;
    }

#if UNITY_EDITOR
    public void OnValidate()
    {
        if (_controllerType == ControllerType.Keyboard)
        {
            _controllerId = 0;
        }
        else
        {
            if (_controllerId <= 0)
            {
                _controllerId = 1;
            }
        }
    }
    [ContextMenu("BITE")]
    private void BITE()
    {
        Debug.Log(Shortcuts.K_MoveForward.Key);
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(P_InputData))]
public class P_InputDataEditor : Editor
{
    private P_InputData Instance;

    private void OnEnable()
    {
        Instance = (P_InputData)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Instance.OnValidate();
    }
}
#endif