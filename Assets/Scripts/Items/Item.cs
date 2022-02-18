using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Default item class
public class Item : MonoBehaviour
{
    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    public float Weight; // Weight of item
    public float Use_Rate; // Use rate
    public Item_Category Category; // Item category - primary, secondary, melee, ...
    public Item_Usage Usage; // Does player need two hands to use this item?
    public bool Switchable; // Can you switch to another weapon while holding this one?
    public Vector3 Default_Rotation; // Default rotation of the current item


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    public virtual bool Primary ()
    {
        return false;
    }

    public virtual bool Secondary () 
    {
        return false;
    }

    public virtual bool Eject () 
    {
        return false;
    }

    public virtual bool Reload () 
    {
        return false;
    }

    public virtual bool Stash () 
    {
        return false;
    }


    //##########################################################################################
    //##################################  DEBUGGING TOSTRING  ##################################
    //##########################################################################################

    // Common item ToString output
    public string CommonToString ()
    {
        string output = "";
        output += "Item classes: " + this.GetType().ToString().Split('_')[1] + " category, " + Category.ToString() + " type, " + Usage.ToString() + " handed, " + (Switchable ? "switchable" : "not switchable") + "\n";
        output += "Item use rate: " + Use_Rate.ToString() + " times/s (" + Math.Round(1/Use_Rate, 2).ToString() + " second period)\n";
        return output;
    }

    // Item ToString output, specific for each script
    public virtual string SpecificToString ()
    {
        return "";
    }

    // Combined ToString output
    public override string ToString()
    {
        return CommonToString() + SpecificToString();
    }

}
