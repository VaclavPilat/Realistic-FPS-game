using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;

// Showing settings
public class Menu_Settings : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    protected GameObject[] Prefabs; // Array of loaded setting prefabs
    protected void Awake () => Prefabs = Resources.LoadAll<GameObject>("Settings/");


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    public string Name; // Setting name


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    protected void Start () => Create();


    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    // Attempts to find a setting prefab by name
    protected GameObject Find_Prefab (string name)
    {
        return Array.Find(Prefabs, g => g.name == name);
    }

    // Regenerating settings
    public IEnumerator Recreate ()
    {
        // Removing current settings
        foreach(Transform row in transform)
            Destroy(row.gameObject);
        // Waiting for the end of the frame
        yield return new WaitForEndOfFrame();
        // Generating new settings
        Create();
        Console.Log(this, "Settings '" + Name + "' regenerated");
    }

    // Generates UI for each setting found
    protected virtual void Create ()
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
                        case "credit": // Showing a credit
                            Generate_Credit(setting, row);
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

    // Generating a label
    protected void Create_Label (GameObject row, string type, string content)
    {
        GameObject label = Instantiate(Find_Prefab(type), row.transform);
        label.GetComponent<Text>().text = content;
    }

    // Generating a label that contains text
    protected void Create_Label_Name (Setting setting, GameObject row) => Create_Label(row, "Name", setting.Name);

    // Generating a label that contains text description of the setting
    protected void Create_Label_Description (Setting setting, GameObject row) => Create_Label(row, "Description", setting.Description);

    // Generating slider with a description label
    protected void Generate_Slider (Setting setting, GameObject row)
    {
        Create_Label(row, "Name", setting.Description);
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
            if(setting.Onchange_Method != null)
                setting.Onchange_Method.Invoke(null, new object[]{ setting });
        });
    }

    // Generating a selection box (dropdown?)
    protected void Generate_Selection (Setting setting, GameObject row)
    {
        Create_Label(row, "Name", setting.Description);
        // Setting up selection box
        GameObject input = Instantiate(Find_Prefab("Selection"), row.transform);
        Dropdown dropdown = input.GetComponent<Dropdown>();
        // Generating options
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
            if(setting.Onchange_Method != null)
                setting.Onchange_Method.Invoke(null, new object[]{ setting });
        });
    }

    // Generating slider with a description label
    protected void Generate_Keybind (Setting setting, GameObject row)
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
    protected void Generate_Checkbox (Setting setting, GameObject row)
    {
        Create_Label(row, "Name", setting.Description);
        // Setting up selection box
        GameObject input = Instantiate(Find_Prefab("Checkbox"), row.transform);
        Toggle toggle = input.GetComponent<Toggle>();
        toggle.isOn = bool.Parse(setting.Value);
        // On edit action
        toggle.onValueChanged.AddListener(delegate { 
            int index = row.transform.GetSiblingIndex();
            Config_Loader.Config[Name][index].Value = toggle.isOn.ToString();
            Config_Loader.Save(Name); 
            if(setting.Onchange_Method != null)
                setting.Onchange_Method.Invoke(null, new object[]{ setting });
        });
    }

    // Generating a CTS record
    protected void Generate_Record (Setting setting, GameObject row)
    {
        // Setting up label with order
        Create_Label(row, "Description", transform.childCount.ToString());
        // Setting up label with time
        Create_Label(row, "Name", setting.Value + " s");
        // Setting up label with datetime
        Create_Label_Description(setting, row);
    }

    // Generating a credit row
    protected void Generate_Credit (Setting setting, GameObject row)
    {
        Create_Label_Name(setting, row);
        Create_Label_Description(setting, row);
    }

}
