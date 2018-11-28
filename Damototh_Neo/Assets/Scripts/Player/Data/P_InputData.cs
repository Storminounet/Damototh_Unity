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
        [SerializeField] private KeyCode k_MoveForward;
        [SerializeField] private KeyCode k_MoveBackward;
        [SerializeField] private KeyCode k_MoveRight;
        [SerializeField] private KeyCode k_MoveLeft;
        [Space]
        [SerializeField] private KeyCode k_Sprint;
        [SerializeField] private KeyCode k_Dodge;
        [Space]
        [SerializeField] private KeyCode k_AutoRotate;
        [SerializeField] private KeyCode k_Lock;
        [Space]
        [SerializeField] private ComputerInput k_LightAttack;
        [SerializeField] private ComputerInput k_HeavyAttack;
        [SerializeField] private ComputerInput k_HydraAttackOne;
        [SerializeField] private ComputerInput k_HydraAttackTwo;

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

        //Keyboard
        public KeyCode K_MoveForward { get { return k_MoveForward; } }
        public KeyCode K_MoveBackward { get { return k_MoveBackward; } }
        public KeyCode K_MoveRight { get { return k_MoveRight; } }
        public KeyCode K_MoveLeft { get { return k_MoveLeft; } }

        public KeyCode K_Sprint { get { return k_Sprint; } }
        public KeyCode K_Dodge { get { return k_Dodge; } }
        public KeyCode K_AutoRotate { get { return k_AutoRotate; } }
        public KeyCode K_Lock { get { return k_Lock; } }

        public ComputerInput K_LightAttack { get { return k_LightAttack; } }
        public ComputerInput K_HeavyAttack { get { return k_HeavyAttack; } }
        public ComputerInput K_HydraAttackOne { get { return k_HydraAttackOne; } }
        public ComputerInput K_HydraAttackTwo { get { return k_HydraAttackTwo; } }


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