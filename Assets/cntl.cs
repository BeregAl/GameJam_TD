using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class cntl : MonoBehaviour
{

    public TextMeshProUGUI ptsText;
    // Start is called before the first frame update
    void Start()
    {
        ptsText.text = PlayerPrefs.GetInt("pts").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
