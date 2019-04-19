using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveInfo", menuName = "TD/WaveInfo")]
public class WaveInfo : ScriptableObject
{
    [Serializable]
    public struct Wave
    {
        public HumanInfo hi;
        public int amount;
    }

    [SerializeField]
    public Wave[] waves;
}
