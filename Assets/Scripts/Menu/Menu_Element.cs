using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// Getting basic element properties
public class Menu_Element : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    [HideInInspector] public GameObject Menu; // Main menu element
    protected Menu_Variables Menu_Variables; // Menu_Variables script

    [HideInInspector] public Image Image = null; // Image component
    [HideInInspector] public Button Button = null; // Button component

    public void Awake ()
    {
        Menu = transform.root.gameObject; // Menu
        Menu_Variables = Menu.GetComponent<Menu_Variables>(); // Menu_Variables script
        Image = transform.GetComponent<Image>(); // Getting Image component
        Button = transform.GetComponent<Button>(); // Getting Button component
    }


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Setting basic stuff
    public virtual void Start()
    {
        // Setting background colors on click
        if(Button)
		    Button.onClick.AddListener(Change_Sprite);
	}

    // Setting sprite to this element
	protected void Change_Sprite () 
    {
        // Setting sprite of all elements to null
        foreach (Menu_Element elementscript in transform.parent.GetComponentsInChildren<Menu_Element>())
        {
            try
            {
                if(elementscript.Button)
                {
                    elementscript.Image.sprite = null;
                    elementscript.Image.color = Color.clear;
                }
            }
            catch {}
        }
        // Setting sprite to this element
        Image.sprite = Menu_Variables.Gradient;
    }

}
