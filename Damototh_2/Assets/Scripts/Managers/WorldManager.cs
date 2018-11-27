using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : Singleton<WorldManager>
{
    [Header("Data")]
    [Space]
    [SerializeField] private WorldData _wData;
    [SerializeField] private P_PlayerController _player;
    [SerializeField] private List<Transform> _enemies;

    [Header("Cheats")]
    [Space]
    [SerializeField] private KeyCode _changeControllerKey = KeyCode.C;


    public static WorldData WData { get { return Instance._wData; } }
    public static List<Transform> Enemies { get { return Instance._enemies; } }

    const float c = 2;
    const float b = 5;
    const float a = b + c;

    private void Awake() 
	{
        SetInstance(this);
        WorldData.SetActiveData(_wData);
	}

    private void Update()
    {
        _wData.UpdateDynamicData();
        HandleCheats();
    }


    private void HandleCheats()
    {
        if (Input.GetKeyDown(_changeControllerKey))
        {
            _player.IData.SetControllerType(
                _player.IData.ControllerType == ControllerType.Keyboard ? ControllerType.PS4 : ControllerType.Keyboard,
                _player.IData.ControllerType == ControllerType.Keyboard ? 0 : 1);
        }
    }


#if UNITY_EDITOR
    [Header("Debug")]
    [Space]
    [SerializeField] private bool _seeHitboxes;

    public static bool SeeHitboxes { get { if (Instance == null) return false; return Instance._seeHitboxes; } }
#endif
}
