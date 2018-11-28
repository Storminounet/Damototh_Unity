using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

	protected static void SetInstance(T instance)
    {
        if (Instance != null)
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying == true)
#endif
                Debug.LogError("Instance already existing on GO : " + instance.gameObject.name + " for script : " + typeof(T));
        }

        Instance = instance;
    }
}
