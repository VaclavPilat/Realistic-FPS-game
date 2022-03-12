using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controlling the current item
public class Human_Item : Lockable_Script
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private Human_Inventory Human_Inventory;
    private void Awake () => Human_Inventory = transform.GetComponent<Human_Inventory>();


    //##########################################################################################
    //###############################  PUBLIC BINDABLE METHODS  ################################
    //##########################################################################################

    [Bindable("Item primary action")]
    public bool Primary () 
    {
        if(Human_Inventory.Current_Item != null)
            return Human_Inventory.Current_Item.GetComponent<Item>().Primary();
        else
            return false;
    }

    [Bindable("Item seconary action")]
    public bool Secondary () 
    {
        if(Human_Inventory.Current_Item != null)
            return Human_Inventory.Current_Item.GetComponent<Item>().Secondary();
        else
            return false;
    }

    [Bindable("Ejects/insert a magazine from the current item")]
    public bool Eject () 
    {
        if(Human_Inventory.Current_Item != null)
            return Human_Inventory.Current_Item.GetComponent<Item>().Eject();
        else
            return false;
    }

    [Bindable("Item reload")]
    public bool Reload () 
    {
        if(Human_Inventory.Current_Item != null)
            return Human_Inventory.Current_Item.GetComponent<Item>().Reload();
        else
            return false;
    }

    [Bindable("Pull out/hide new magazine")]
    public bool Stash () 
    {
        if(Human_Inventory.Current_Item != null)
            return Human_Inventory.Current_Item.GetComponent<Item>().Stash();
        else
            return false;
    }


    //##########################################################################################
    //##################################  DEBUGGING TOSTRING  ##################################
    //##########################################################################################

    public override string ToString ()
    {
        string output = "";
        return output;
    }

}
