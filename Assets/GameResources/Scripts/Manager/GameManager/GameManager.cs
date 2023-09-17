using GameController.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] SEManager seManager;

    //--------------------------------------------------

    void Awake()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            seManager.PlayAudio("ButtonClick");
        }
    }
}
