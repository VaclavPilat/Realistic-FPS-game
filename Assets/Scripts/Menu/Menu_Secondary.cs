using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// Applying rules to secondary (normal) elements
public class Menu_Secondary : Menu_Element
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private GameObject Contents;


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
            Image.color = Menu_Variables.Secondary_Color;
        // Setting background colors on click
        if(Button)
        {
            var contents = transform.parent.parent.parent.parent.parent.Find("Contents");
            if(contents)
            {
                Contents = contents.gameObject;
                if(transform.GetSiblingIndex() == 0)
                {
                    Change_Sprite();
                    Show_Content();
                }
            }
            else
                Console.Error(this, "Cannot find contents object");
		    Button.onClick.AddListener(Show_Content);
        }
	}

    // Setting some stuff after button is clicked
    private void Show_Content ()
    {
        // Setting contents active
        Contents.SetActive(true);
        // Setting image color
        if(Image)
            Image.color = Menu_Variables.Tertiary_Color;
        // Hiding contents
        for(int i = 0; i < Contents.transform.childCount; i++)
        {
            Contents.transform.GetChild(i).gameObject.SetActive(false);
        }
        // Showing current content
        Transform content = Contents.transform.Find(transform.name);
        if(content)
            content.gameObject.SetActive(true);
        else
            Console.Warning(this, "Secondary menu doesn't have a corresponding panel");
    }

}
