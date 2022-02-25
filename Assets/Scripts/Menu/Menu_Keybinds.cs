using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Loading keybinds
public class Menu_Keybinds : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    public GameObject Keybind; // Keybind UI prefab
    private Menu_Variables Menu_Variables; // Menu variables


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private void Awake () 
    {
        Menu_Variables = transform.root.GetComponent<Menu_Variables>();
        Create_UI();
    }


    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    // Generates UI for each keybind found
    private void Create_UI ()
    {
        if(Menu_Variables != null)
            foreach(Keybind keybind in Menu_Variables.Keybinds)
            {
                GameObject bind = Instantiate(Keybind, transform);
                // Setting UI
                bind.transform.GetChild(0).GetComponent<InputField>().text = keybind.Name;
                bind.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = keybind.KeyCode.ToString();
                bind.transform.GetChild(2).GetComponent<InputField>().text = keybind.KeyDown;
                bind.transform.GetChild(3).GetComponent<InputField>().text = keybind.Key;
                bind.transform.GetChild(4).GetComponent<InputField>().text = keybind.KeyUp;
            }
        // Resizing scrollview
        transform.GetComponent<Menu_Scrollview>().Resize();
    }

}
