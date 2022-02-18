using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Testobject
{
    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    public Transform Prefab; // Gameobject prefab
    public Vector3 Start; // Start position
    public int Count; // Number of generated objects from this prefab
    public Vector3 Position; // Position increment
    public Vector3 Scale; // Object size multiple increment
    public Vector3 Rotation; // Object rotation increment
}
