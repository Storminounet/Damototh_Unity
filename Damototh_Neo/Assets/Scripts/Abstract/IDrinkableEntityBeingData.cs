﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public interface IDrinkableEntityBeingData 
{
    float DrinkableBlood { get; }
}