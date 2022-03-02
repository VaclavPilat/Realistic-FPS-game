using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// Showing settings
public class Menu_Settings : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    public GameObject Row; // Row prefab

    // Inputs
    public GameObject Slider; // Slider input


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
            GameObject row = Instantiate(Row, transform);
            row.transform.GetChild(0).GetComponent<Text>().text = setting.Description;
            // Setting correct input type
            GameObject input;
            switch(setting.Type)
            {
                case "float":
                    input = Instantiate(Slider, row.transform);
                    float hodnota = 0.5f;
                    if(!float.TryParse(setting.Value, out hodnota))
                        Console.Warning(this, "Cannot convert value of setting '" + setting.Name + "' to " + setting.Type);
                    input.GetComponent<Slider>().value = hodnota;
                    break;
                default:
                    break;
            }
            // Resizing elements in row
            row.GetComponent<Menu_Gridlayout>().Resize();
        }
        // Resizing scrollview
        transform.GetComponent<Menu_Scrollview>().Resize();
    }

}
