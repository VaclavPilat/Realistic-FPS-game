using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Tablet with information about the character that was using it
public class Item_Tablet : Item
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    public GameObject Screen; // Canvas representing the screen of the tablet


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private GameObject Character = null; // Character whose information is being shown

    private void Start () => Screen.SetActive(false); // Disabling tablet


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################
    
    // Showing information
    private void Update ()
    {
        // Checking if someone has this tablet in his inventory
        if(transform.parent == null)
        {
            if(Character != null)
                Disconnect();
        }
        else
        {
            if(Character == null)
                Connect();
        }
    }

    // Connectring tablet to character
    private void Connect ()
    {
        Character = transform.root.gameObject;
        Screen.SetActive(true);
        Console.Log(this, "Tablet connected to \"" + Character.transform.name + "\"");
    }

    // Disconnecting tablet from character
    private void Disconnect ()
    {
        Console.Log(this, "Tablet disconnected from \"" + Character.transform.name + "\"");
        Screen.SetActive(false);
        Character = null;
    }

}
