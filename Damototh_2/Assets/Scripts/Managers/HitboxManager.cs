using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
public class HitboxManager : MonoBehaviour 
{
    public const int TOTAL_ATTACK_NUMBER = 8;

    public enum HitboxType
    {
        LightAttack_1 = 0,
        LightAttack_2 = 1,
        LightAttack_3 = 2,
        HeavyAttack_1 = 3,
        HeavyAttack_2 = 4,
        HeavyAttack_3 = 5,
        Hydra_1 = 6,
        Hydra_2 = 7
    }

    [SerializeField] private HitboxType _editedHitbox = HitboxType.LightAttack_1;
    [Space]
    [SerializeField] private Transform _visualDisplayer;
    [SerializeField] private GameObject[] _hitboxes;

    private HitboxType _curHitboxType = HitboxType.LightAttack_1;

    private GameObject _currentHitboxGameObject;
    private Transform _currentHitbox;
    private BoxCollider _currentBox;
    private Transform _currentTransform;

    public Vector3 HitboxCenter;
    public Vector3 HitboxSizes;
    public Vector3 HitboxRotation;

    public GameObject CurrentHitboxGameObject { get { return _currentHitboxGameObject; } }
    public Transform CurrentTransform { get { return _currentTransform; } }

    private void StartEditHitbox(HitboxType type)
    {
        RemoveEditedHitbox();

        _currentHitboxGameObject = (GameObject)PrefabUtility.InstantiatePrefab(_hitboxes[(int)type]);
        _currentTransform = _currentHitboxGameObject.transform;
        _currentTransform.position = Vector3.zero;
        _currentTransform.rotation = Quaternion.identity;
        _currentTransform.parent = (transform);

        _currentHitbox = _currentTransform.Find("Hitbox");
        _currentBox = _currentHitbox.GetComponent<BoxCollider>();

        HitboxCenter = _currentHitbox.localPosition;
        HitboxSizes = _currentBox.size;
        HitboxRotation = _currentHitbox.localEulerAngles;
    }

    private void RemoveEditedHitbox()
    {
        if (_currentHitboxGameObject != null)
        {
            StartCoroutine(LateDestroy(_currentHitboxGameObject));
            _currentHitboxGameObject = null;
            _currentTransform = null;
        }
    }

    public void OnValidate()
    {
        if (_currentHitboxGameObject == null)
        {
            if (GetComponentInChildren<Attack>() != null)
            {
                StartCoroutine(LateDestroy(GetComponentInChildren<Attack>().gameObject));
            }

            StartEditHitbox(HitboxType.LightAttack_1);
        }

        if (_curHitboxType != _editedHitbox)
        {
            _curHitboxType = _editedHitbox;
            StartEditHitbox(_editedHitbox);
        }

        _currentHitbox.localPosition = HitboxCenter;
        _currentBox.size = HitboxSizes;
        _currentHitbox.localEulerAngles = HitboxRotation;

        _visualDisplayer.position = _currentHitbox.position;
        _visualDisplayer.localScale = _currentBox.size;
        _visualDisplayer.eulerAngles = _currentHitbox.eulerAngles;
    }

    IEnumerator LateDestroy(GameObject o)
    {
        yield return null;
        DestroyImmediate(o);
    }

    public bool Advanced;
    public HitboxType EditedHitbox { get { return _editedHitbox; } set { _editedHitbox = value; } }
    public Transform VisualDisplayer { get { return _visualDisplayer; } set { _visualDisplayer = value; } }
    public GameObject[] Hitboxes { get { return _hitboxes; } set { _hitboxes = value; } }
}

[CustomEditor(typeof(HitboxManager))]
public class HitboxManagerEditor : Editor
{
	private HitboxManager Instance;

    private void OnEnable()
    {
        Instance = (HitboxManager)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Save Hitbox"))
        {
            PrefabUtility.ReplacePrefab(
                Instance.CurrentHitboxGameObject,
                PrefabUtility.GetCorrespondingObjectFromSource(Instance.CurrentHitboxGameObject),
                ReplacePrefabOptions.ConnectToPrefab);
        }
        GUILayout.Space(17);
        Undo.RecordObject(Instance, "Hitbox Type Change");
        Instance.EditedHitbox = (HitboxManager.HitboxType)EditorGUILayout.EnumPopup("Edited Hitbox", Instance.EditedHitbox);

        GUILayout.Space(17);
        GUILayout.Label("Hitbox Center");
        Undo.RecordObject(Instance, "Hitbox Center Change");
        Instance.HitboxCenter.x = EditorGUILayout.Slider("Horizontal :", Instance.HitboxCenter.x, -2f, 2f);
        Instance.HitboxCenter.y = EditorGUILayout.Slider("Vertical :", Instance.HitboxCenter.y, -2f, 2f);
        Instance.HitboxCenter.z = EditorGUILayout.Slider("Forward :", Instance.HitboxCenter.z, -2f, 2f);

        GUILayout.Space(17);
        GUILayout.Label("Hitbox Size");
        Undo.RecordObject(Instance, "Hitbox Width Change");
        Instance.HitboxSizes.x = EditorGUILayout.Slider("Width :", Instance.HitboxSizes.x, 0.01f, 5f);
        Undo.RecordObject(Instance, "Hitbox Height Change");
        Instance.HitboxSizes.y = EditorGUILayout.Slider("Height :", Instance.HitboxSizes.y, 0.01f, 5f);
        Undo.RecordObject(Instance, "Hitbox Depth Change");
        Instance.HitboxSizes.z = EditorGUILayout.Slider("Depth :", Instance.HitboxSizes.z, 0.01f, 5f);

        GUILayout.Space(17);
        GUILayout.Label("Hitbox Rotation");
        Undo.RecordObject(Instance, "Hitbox Pitch Change");
        Instance.HitboxRotation.x = EditorGUILayout.Slider("Pitch :", Instance.HitboxRotation.x, 0f, 360f);
        Undo.RecordObject(Instance, "Hitbox Yaw Change");
        Instance.HitboxRotation.y = EditorGUILayout.Slider("Yaw :", Instance.HitboxRotation.y, 0f, 360f);
        Undo.RecordObject(Instance, "Hitbox Roll Change");
        Instance.HitboxRotation.z = EditorGUILayout.Slider("Roll :", Instance.HitboxRotation.z, 0f, 360f);


        GUILayout.Space(100);

        Instance.Advanced = EditorGUILayout.Toggle("Advanced Mode", Instance.Advanced);
        if (Instance.Advanced)
        {
            Instance.VisualDisplayer = (Transform)EditorGUILayout.ObjectField("Visual Displayer", Instance.VisualDisplayer, typeof(Transform), false);

            GUILayout.Space(17);
            if (Instance.Hitboxes == null)
            {
                Instance.Hitboxes = new GameObject[HitboxManager.TOTAL_ATTACK_NUMBER];
            }
            for (int i = 0; i < HitboxManager.TOTAL_ATTACK_NUMBER; i++)
            {
                Instance.Hitboxes[i] = (GameObject)EditorGUILayout.ObjectField(System.Enum.GetNames(typeof(HitboxManager.HitboxType))[i], Instance.Hitboxes[i], typeof(GameObject), false);
            }
        }
        Instance.OnValidate();
    }
}
#endif
