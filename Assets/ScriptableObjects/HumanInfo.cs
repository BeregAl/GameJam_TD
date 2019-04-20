using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HumanInfo",menuName = "TD/HumanInfo")]
public class HumanInfo : ScriptableObject
{
    public string humanName;
    public float health;
    public float speed;

    public GameObject humanPrefab;

}
