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

    private GameObject[] Prefabs; // Array of loaded setting prefabs
    private void Awake () => Prefabs = Resources.LoadAll<GameObject>("Settings/");


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

    // Attempts to find a setting prefab by name
    private GameObject Find_Prefab (string name)
    {
        return Array.Find(Prefabs, g => g.name == name);
    }

    // Generates UI for each setting found
    private void Create_UI ()
    {
        if(Config_Loader.Load(Name))
            foreach(Setting setting in Config_Loader.Config[Name])
            {
                try
                {
                    GameObject row = Instantiate(Find_Prefab("Row"), transform);
                    // Setting correct input type
                    switch(setting.Type)
                    {
                        case "slider": // Showing sliders for float values
                            Generate_Slider(setting, row);
                            break;
                        case "keybind": // Generating keybinds
                            Generate_Keybind(setting, row);
                            break;
                        case "selection": // Showing boxes withc selection (dropdowns?)
                            Generate_Selection(setting, row);
                            break;
                        case "checkbox": // Showing a simple checkbox
                            Generate_Checkbox(setting, row);
                            break;
                        case "record": // Showing a CTS record
                            Generate_Record(setting, row);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.Error(this, "Failed generating setting row; " + e.Message);
                }
            }
        // Resizing scrollview
        transform.GetComponent<Menu_Scrollview>().Resize();
    }

    // Generating a label that contains text description of the setting
    private void Create_Title (Setting setting, GameObject row)
    {
        // Setting up label
        GameObject label = Instantiate(Find_Prefab("Description"), row.transform);
        label.GetComponent<Text>().text = setting.Description;
    }

    // Generating slider with a description label
    private void Generate_Slider (Setting setting, GameObject row)
    {
        Create_Title(setting, row);
        // Setting up slider
        GameObject input = Instantiate(Find_Prefab("Slider"), row.transform);
        Slider slider = input.GetComponent<Slider>();
        string[] check = setting.Check.Split('|');
        slider.minValue = float.Parse(check[0]);
        slider.maxValue = float.Parse(check[1]);
        slider.value = float.Parse(setting.Value);
        // On edit action
        slider.onValueChanged.AddListener(delegate { 
            int index = row.transform.GetSiblingIndex();
            Config_Loader.Config[Name][index].Value = slider.value.ToString();
            Config_Loader.Save(Name); 
        });
    }

    // Generating a selection box (dropdown?)
    private void Generate_Selection (Setting setting, GameObject row)
    {
        Create_Title(setting, row);
        // Setting up selection box
        GameObject input = Instantiate(Find_Prefab("Selection"), row.transform);
        Dropdown dropdown = input.GetComponent<Dropdown>();
        foreach(string text in setting.Check.Split('|'))
        {
            var option = new Dropdown.OptionData();
            option.text = text;
            dropdown.options.Add(option);
        }
        dropdown.value = int.Parse(setting.Value);
        // On edit action
        dropdown.onValueChanged.AddListener(delegate { 
            int index = row.transform.GetSiblingIndex();
            Config_Loader.Config[Name][index].Value = dropdown.value.ToString();
            Config_Loader.Save(Name); 
        });
    }

    // Generating slider with a description label
    private void Generate_Keybind (Setting setting, GameObject row)
    {
        // Keybind description
        GameObject description = Instantiate(Find_Prefab("InputField"), row.transform);
        description.GetComponent<InputField>().text = setting.Description;
        description.transform.GetChild(0).GetComponent<Text>().fontStyle = FontStyle.Bold;
        // Keycode
        GameObject keycode = Instantiate(Find_Prefab("Button"), row.transform);
        keycode.transform.GetChild(0).GetComponent<Text>().text = ((KeyCode)int.Parse(setting.Name)).ToString();
        // KeyDown actions
        string[] actions = setting.Value.Split('|');
        GameObject keydown = Instantiate(Find_Prefab("InputField"), row.transform);
        keydown.GetComponent<InputField>().text = actions[0];
        // Key actions
        GameObject key = Instantiate(Find_Prefab("InputField"), row.transform);
        key.GetComponent<InputField>().text = actions[1];
        // KeyUp actions
        GameObject keyup = Instantiate(Find_Prefab("InputField"), row.transform);
        keyup.GetComponent<InputField>().text = actions[2];
        // Adding script for handling keybinds
        row.AddComponent<Menu_Keybind>();
    }

    // Generating a checkbox
    private void Generate_Checkbox (Setting setting, GameObject row)
    {
        Create_Title(setting, row);
        // Setting up selection box
        GameObject input = Instantiate(Find_Prefab("Checkbox"), row.transform);
        Toggle toggle = input.GetComponent<Toggle>();
        toggle.isOn = bool.Parse(setting.Value);
        // On edit action
        toggle.onValueChanged.AddListener(delegate { 
            int index = row.transform.GetSiblingIndex();
            Config_Loader.Config[Name][index].Value = toggle.isOn.ToString();
            Config_Loader.Save(Name); 
        });
    }

    // Generating a CTS record
    private void Generate_Record (Setting setting, GameObject row)
    {
        // Setting up label with order
        GameObject order = Instantiate(Find_Prefab("Description"), row.transform);
        order.GetComponent<Text>().text = transform.childCount.ToString();
        // Setting up label with time
        GameObject time = Instantiate(Find_Prefab("Description"), row.transform);
        time.GetComponent<Text>().text = setting.Value + " s";
        // Setting up label with datetime
        Create_Title(setting, row);
    }

}
