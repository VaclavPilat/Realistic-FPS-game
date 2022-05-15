using UnityEngine;
using UnityEngine.UI;
using System;

// Rotating a loading circle
public class Menu_Loading : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private RectTransform RectTransform;
    private void Awake() => RectTransform = GetComponent<RectTransform>();


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private float Speed = 200f;


    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    // Rotating image
    private void Update() => RectTransform.Rotate(0f, 0f, Speed * Time.deltaTime);

}
