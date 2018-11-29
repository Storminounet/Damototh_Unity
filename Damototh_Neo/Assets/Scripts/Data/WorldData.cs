using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldData", menuName = "GlobalData/WorldData", order = 1000)]
public class WorldData : DataSingleton<WorldData>
{
    [Header("Layers")]
    [Space]
    [SerializeField] private LayerMask _defaultSolidLayer;
    [SerializeField] private LayerMask _interactableLayer;

    [Header("Others")]
    [Space]
    [SerializeField] private Material _transparentMat;

    private float _time;
    private float _deltaTime;
    private float _fixedDeltaTime;

    public void UpdateDynamicData()
    {
        _time = UnityEngine.Time.time;
        _deltaTime = UnityEngine.Time.deltaTime;
        _fixedDeltaTime = UnityEngine.Time.fixedDeltaTime;
    }

    public static LayerMask DefaultSolidLayer { get { return ActiveData._defaultSolidLayer; } }
    public static LayerMask InteractableLayer { get { return ActiveData._interactableLayer; } }

    public static Material TransparentMat { get { return ActiveData._transparentMat; } }


    public static float Time { get { return ActiveData._time; } }
    public static float DeltaTime { get { return ActiveData._deltaTime; } }
    public static float FixedDeltaTime { get { return ActiveData._fixedDeltaTime; } }
}

