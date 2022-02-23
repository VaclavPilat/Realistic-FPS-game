using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Reflection;

// Showing bindable methods
public class Menu_Bindable : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    public GameObject Row; // Row prefab


    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    private void Awake () => Show_Bindable();
    
    // Generating UI for bindable methods
    private void Show_Bindable ()
    {
        // Generating GUI
        foreach(KeyValuePair<string, MethodInfo> bindable in transform.root.GetComponent<Menu_Variables>().Bindable)
        {
            // Showing bindable method name
            GameObject row = Instantiate(Row, transform);
            row.transform.GetChild(0).GetComponent<Text>().text = bindable.Key;
            // Showing bindable method description
            row.transform.GetChild(1).GetComponent<Text>().text = ((Bindable) bindable.Value.GetCustomAttributes(typeof(Bindable), false)[0]).Description;
        }
        // Resizing parent element
        transform.GetComponent<Menu_Scrollview>().Resize();
    }

}
