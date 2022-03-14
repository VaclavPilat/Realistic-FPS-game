using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tracks current health state
public class Human_Health : Lockable_Script
{
    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private float Maximum_Blood = 5000f; // Maximum blood amount in milileters
    private float Blood = 0f; // Current amount of blood in milileters
    private float Bleeding = 0f; // Bleeding speed (ml/s)
    
    // List of colliders
    [SerializeField] private Collider[] Head; 
    [SerializeField] private Collider[] Left_Arm; 
    [SerializeField] private Collider[] Right_Arm; 
    [SerializeField] private Collider[] Chest;
    [SerializeField] private Collider[] Left_Leg;
    [SerializeField] private Collider[] Right_Leg;


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################
    
    public bool Alive = true; // Is character alive?


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    private void Awake () 
    {
        // Setting blood volume
        Blood = Maximum_Blood;
    }


    //##########################################################################################
    //##################################  DEBUGGING TOSTRING  ##################################
    //##########################################################################################

    public override string ToString ()
    {
        string output = "";
        output += "Blood: " + Blood.ToString() + "/" + Maximum_Blood.ToString() + " ml (" + ((Blood / Maximum_Blood) * 100f).ToString() + "%)\n";
        output += "Bleeding: " + Bleeding.ToString() + " ml/s\n";
        return output;
    }

}
