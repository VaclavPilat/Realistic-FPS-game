using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Melee weapons
public class Item_Melee : Item
{
    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private float Charge; // Charge percentage multiplier


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    public float Bleeding; // Bleeding caused by a shot


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Melee swing
    public override bool Primary ()
    {
        return true;
    }

    // Charging melee attack multiplier
    public override bool Secondary () 
    {
        return true;
    }


    //##########################################################################################
    //##################################  DEBUGGING TOSTRING  ##################################
    //##########################################################################################

    public override string SpecificToString ()
    {
        string output = "";
        output += "Bleeding caused by item: " + Bleeding.ToString() + " ml/s\n";
        output += "Charge multiplier: +" + Math.Round(Charge*100, 2).ToString() + "%\n";
        return output;
    }

}
