using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Showing settings
public class Menu_Settings : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    public GameObject Row; // Row prefab


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    public string Name; // Setting name


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private Setting[] Settings;

    private void Awake () 
    {
        Settings = Config_Loader.Load<Setting>(Name);
        Create_UI();
    }


    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    // Generates UI for each setting found
    private void Create_UI ()
    {
        foreach(Setting setting in Settings)
        {
            GameObject s = Instantiate(Row, transform);
            s.transform.GetChild(0).GetComponent<Text>().text = setting.Description;
        }
        // Resizing scrollview
        transform.GetComponent<Menu_Scrollview>().Resize();
    }

}
