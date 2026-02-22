using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class folllowMc : MonoBehaviour
{
    public cameraAncor player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) 
        { 
            transform.position = player.transform.position;
        }
    }

    public void Ikut()
    {
        player = FindAnyObjectByType<cameraAncor>();
        return;
    }
}
