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

    private void Awake () => Create_UI();


    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    // Generates UI for each setting found
    private void Create_UI ()
    {
        Config_Loader.Load(Name);
        foreach(Setting setting in Config_Loader.Config[Name])
        {
            try
            {
                GameObject row = Instantiate(Row, transform);
                row.transform.GetChild(0).GetComponent<Text>().text = setting.Description;
                // Setting correct input type
                GameObject input;
                switch(setting.Type)
                {
                    case "float": // Showing sliders for float values
                        input = Instantiate(Slider, row.transform);
                        var slider = input.GetComponent<Slider>();
                        string[] check = setting.Check.Split('|');
                        slider.minValue = float.Parse(check[0]);
                        slider.maxValue = float.Parse(check[1]);
                        slider.value = float.Parse(setting.Value);
                        break;
                    default:
                        break;
                }
                // Resizing elements in row
                row.GetComponent<Menu_Gridlayout>().Resize();
            }
            catch (Exception e)
            {
                Console.Error(this, "Invalid configuration of Audio settings");
            }
        }
        // Resizing scrollview
        transform.GetComponent<Menu_Scrollview>().Resize();
    }

}
