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
    public GameObject Description; // Label
    public GameObject Slider; // Slider input
    public GameObject InputField; // Text input field
    public GameObject Button; // Button

    // Getting input prefabs from resources
    private void Awake ()
    {
        Row          = Resources.Load<GameObject>("Settings/Row");
        Description  = Resources.Load<GameObject>("Settings/Description");
        Slider       = Resources.Load<GameObject>("Settings/Slider");
        InputField   = Resources.Load<GameObject>("Settings/InputField");
        Button       = Resources.Load<GameObject>("Settings/Button");
    }


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    public string Name; // Setting name


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private void Start () => Create_UI();


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
                // Setting correct input type
                switch(setting.Type)
                {
                    case "float": // Showing sliders for float values
                        Generate_Slider(setting, row);
                        break;
                    default:
                        break;
                }
                // Resizing elements in row
                row.GetComponent<Menu_Gridlayout>().Resize();
            }
            catch (Exception e)
            {
                Console.Error(this, "Failed generating setting row; " + e.Message);
            }
        }
        // Resizing scrollview
        transform.GetComponent<Menu_Scrollview>().Resize();
    }

    // Generating slider with a description label
    private void Generate_Slider (Setting setting, GameObject row)
    {
        // Setting up label
        GameObject label = Instantiate(Description, row.transform);
        label.GetComponent<Text>().text = setting.Description;
        // Setting up slider
        GameObject input = Instantiate(Slider, row.transform);
        var slider = input.GetComponent<Slider>();
        string[] check = setting.Check.Split('|');
        slider.minValue = float.Parse(check[0]);
        slider.maxValue = float.Parse(check[1]);
        slider.value = float.Parse(setting.Value);
        slider.onValueChanged.AddListener(delegate { 
            int index = slider.transform.parent.GetSiblingIndex();
            Config_Loader.Config[Name][index].Value = slider.value.ToString();
            Config_Loader.Save(Name); 
        });
    }

}
