using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// Applying rules to primary (darkest) elements
public class Menu_Primary : Menu_Element
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
            Image.color = Menu_Variables.Primary_Color;
        // Setting background colors on click
        if(Button)
		    Button.onClick.AddListener(Show_Content);
	}

    // Setting some stuff after button is clicked
    private void Show_Content ()
    {
        // Setting contents active
        Menu_Variables.Contents.SetActive(true);
        // Setting image color
        if(Image)
            Image.color = Menu_Variables.Secondary_Color;
        // Hiding contents
        for(int i = 0; i < Menu_Variables.Contents.transform.childCount; i++)
        {
            Menu_Variables.Contents.transform.GetChild(i).gameObject.SetActive(false);
        }
        // Showing current content
        Transform content = Menu_Variables.Contents.transform.Find(transform.name);
        if(content)
            content.gameObject.SetActive(true);
        else
            Menu_Variables.Contents.SetActive(false);
    }

}
