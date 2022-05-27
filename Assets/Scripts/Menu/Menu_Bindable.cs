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

    private void Start ()
    {
        // Generating GUI
        foreach(KeyValuePair<string, MethodInfo> bindable in transform.root.GetComponent<Menu_Variables>().Bindable)
        {
            // Creating row
            GameObject row = Instantiate(Find_Prefab("Row"), transform);
            // Bindable method name
            GameObject name = Instantiate(Find_Prefab("Name"), row.transform);
            name.GetComponent<Text>().text = bindable.Key;
            // Bindable method description
            GameObject description = Instantiate(Find_Prefab("Description"), row.transform);
            description.GetComponent<Text>().text = ((Bindable) bindable.Value.GetCustomAttributes(typeof(Bindable), false)[0]).Description;
        }
        // Resizing parent element
        transform.GetComponent<Menu_Scrollview>().Resize();
    }

}
