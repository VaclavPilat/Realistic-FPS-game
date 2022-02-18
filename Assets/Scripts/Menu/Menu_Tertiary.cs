using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// Applying rules to tertiary (lightest) elements
public class Menu_Tertiary : Menu_Element
{
    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Setting basic stuff
    public override void Start()
    {
        // Calling Start method of a parent class
        base.Start();
        // Setting background color
        if(Image && !Button)
            Image.color = Menu_Variables.Tertiary_Color;
	}

}
