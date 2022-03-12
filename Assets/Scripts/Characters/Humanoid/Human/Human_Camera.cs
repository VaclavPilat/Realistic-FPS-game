using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human_Camera : Lockable_Script
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    [SerializeField] private GameObject Cam; // Gameobject with a camera component on it
    [HideInInspector] public Camera Camera;
    [HideInInspector] public AudioListener AudioListener;
    [HideInInspector] public Transform Item_Position;

    private void Awake ()
    {
        Camera = Cam.GetComponent<Camera>();
        AudioListener = Cam.GetComponent<AudioListener>();
        Item_Position = Cam.transform.GetChild(0);
    }

}