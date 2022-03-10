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

    private void Awake () => Blood = Maximum_Blood;


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################
    
    public bool Alive = true; // Is character alive?


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
