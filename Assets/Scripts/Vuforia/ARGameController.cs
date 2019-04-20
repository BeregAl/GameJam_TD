using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARGameController : MonoBehaviour {
    // Контроллер для основных механик игры и ссылок на основные игровые объекты
    public static ARGameController instance;
    
    [Header("Игровые Префабы")]
    // Ссылки на игровые префабы   
    public GameObject roadPref;

    [Header("Ссылки на основные игровые объекты")]
    // Ссылки на основные игровые объекты   
    public GameObject mainBase;    
    public GameObject mainSpawnPoint;    
    public GameObject mainRoadNode;

    [Header("Ссылки на основные AR объекты")]
    // Ссылки на основные AR объекты
    public Transform mainTargetImage;


    private void Start()
    {
        instance = this;
    }


}
