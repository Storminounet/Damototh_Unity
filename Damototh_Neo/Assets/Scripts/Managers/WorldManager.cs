using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : Singleton<WorldManager>
{
    [Header("Data")]
    [Space]
    [SerializeField] private WorldData _wData;
    [SerializeField] private P_PlayerController _player;

    [Header("Cheats")]
    [Space]
    [SerializeField] private KeyCode _changeControllerKey = KeyCode.C;

    public static WorldData WData { get { return Instance._wData; } }
    public static P_PlayerController Player { get { return Instance._player; } }

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

        if (Input.GetKeyDown(KeyCode.M))
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }


    //Events
    //PlayerEvents
    public static void OnPlayerLock(EntityController entity)
    {
        PostProcessManager.OnPlayerLock();
    }
    public static void OnPlayerUnlock()
    {
        PostProcessManager.OnPlayerUnlock();
    }

    public static void OnPlayerStartDodging()
    {
        OnEntityStartDodging(Player);
    }
    public static void OnPlayerEndDodging()
    {
        OnEntityEndDodging(Player);
    }

    public static void OnPlayerStartAttacking(AttackData attack)
    {
        OnEntityStartAttacking(Player, attack);
    }
    public static void OnPlayerEndAttacking()
    {
        OnEntityEndAttacking(Player);
    }
    public static void OnPlayerHit(EntityController hitEntity, AttackData hitAttack)
    {
        PostProcessManager.OnPlayerHitEntity();
        CameraShakeManager.OnPlayerHitEntity(hitAttack);

        IAManager.OnEntityHit(Player, hitEntity, hitAttack);
    }
    public static void OnPlayerKill(EntityController killedEntity, AttackData killAttack)
    {
        PostProcessManager.OnPlayerKillEntity();
        CameraShakeManager.OnPlayerKillEntity(killAttack);

        IAManager.OnEntityKill(Player, killedEntity, killAttack);
    }


    //Entity events
    public static void OnEntityStartDodging(EntityController entity)
    {
        IAManager.OnEntityStartDodging(entity);
    }
    public static void OnEntityEndDodging(EntityController entity)
    {
        IAManager.OnEntityEndDodging(entity);
    }

    public static void OnEntityStartAttacking(EntityController entity, AttackData attack)
    {
        IAManager.OnEntityStartAttacking(entity, attack);
    }
    public static void OnEntityEndAttacking(EntityController entity)
    {
        IAManager.OnEntityEndAttacking(entity);
    }


    public static void OnEntityDeath(EntityController entity)
    {
        IAManager.OnEntityDeath(entity);
    }
    public static void OnEntityDrank(EntityController entity)
    {
        IAManager.OnEntityDrank(entity);
    }

    //Others
    private void HandleCheats()
    {
        if (Input.GetKeyDown(_changeControllerKey))
        {
            _player.IpData.SetControllerType(
                _player.IpData.ControllerType == ControllerType.Keyboard ? ControllerType.PS4 : ControllerType.Keyboard,
                _player.IpData.ControllerType == ControllerType.Keyboard ? 0 : 1);
        }
    }

#if UNITY_EDITOR
    [Header("Debug")]
    [Space]
    [SerializeField] private bool _seeHitboxes;
    [SerializeField] private bool _seeHealthBar;
    [SerializeField] private bool _seeStunBar;
    [SerializeField] private float _healthBarSize = 1;
    [SerializeField] private float _stunBarSize = 1;

    public static bool SeeHitboxes { get { if (Instance == null) return false; return Instance._seeHitboxes; } }
    public static bool SeeHealthBar { get { if (Instance == null) return false; return Instance._seeHealthBar; } }
    public static bool SeeStunBar { get { if (Instance == null) return false; return Instance._seeStunBar; } }

    public static float HealthBarSize { get { if (Instance == null) return 0f; return Instance._healthBarSize; } }
    public static float StunBarSize { get { if (Instance == null) return 0f; return Instance._stunBarSize; } }

    public static void EditorForceAwake()
    {
        FindObjectOfType<WorldManager>().Awake();
    }

    private void OnValidate()
    {
        if (_healthBarSize < 0.001f)
        {
            _healthBarSize = 0.001f;
        }

        if (_stunBarSize < 0.001f)
        {
            _stunBarSize = 0.001f;
        }
    }
#endif
}
