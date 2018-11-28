using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSingleton<T> : ScriptableObject 
{
	public static T ActiveData { get; private set; }

    public static void SetActiveData(T data)
    {
        ActiveData = data;
    }
}
