using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;

// Showing bindable methods
public class Menu_Bindable : Menu_Settings
{
    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    protected override void Create ()
    {
        // Generating GUI
        foreach(KeyValuePair<string, MethodInfo> bindable in transform.root.GetComponent<Menu_Variables>().Bindable)
        {
            // Creating row
            GameObject row = Instantiate(Find_Prefab("Row"), transform);
            // Bindable method name
            Create_Label(row, "Name", bindable.Key);
            // Bindable method description
            Create_Label(row, "Description", ((Bindable) bindable.Value.GetCustomAttributes(typeof(Bindable), false)[0]).Description);
        }
        // Resizing parent element
        transform.GetComponent<Menu_Scrollview>().Resize();
    }

}
