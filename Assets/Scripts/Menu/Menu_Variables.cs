using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;

// Variables for UI elements
public class Menu_Variables : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    [HideInInspector] public Image Image = null; // Image component
    public GameObject Contents; // Gameobject that contains secondary contents
    private CanvasGroup CanvasGroup; // Canvas group


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    public Sprite Gradient; // Gradient image

    [HideInInspector] public Color Primary_Color, Secondary_Color, Tertiary_Color;


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private bool Switchable; // Is menu visibility switchable?
    private bool Visible; // Is menu visible?

    private string Resource = "Config/Keybinds"; // Name of resource that stores keybinds
    [HideInInspector] public Keybind[] Keybinds = null; // Array of keybinds

    [SerializeField] private GameObject Player_Prefab; // Player prefab (for getting bindable methods)
    [HideInInspector] public Dictionary<string, MethodInfo> Bindable = new Dictionary<string, MethodInfo>(); // Dictionary with all bindable methods this player can use


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    private void Awake ()
    {
        // Getting Image component
        Image = GetComponent<Image>(); 
        // Disabling contents
        Contents.SetActive(false);
        // Setting canvas group
        CanvasGroup = GetComponent<CanvasGroup>();
        // Setting colors
        Primary_Color = new Color(29f/255f, 29f/255f, 29f/255f, 1f);
        Secondary_Color = new Color(130f/255f, 130f/255f, 130f/255f, 0.4f);
        Tertiary_Color = new Color(1f, 1f, 1f, 0.3f);
        // Disabling menu when a new scene is loaded
        SceneManager.sceneLoaded += Hide_On_Load;
        // Loading keybinds for later use
        Load_Keybinds();
        // Loads all bindable methods into a dictionary
        Load_Bindable();
    }

    // Hiding menu on most scenes
    private void Hide_On_Load (Scene scene, LoadSceneMode mode)
    {
        switch(scene.name)
        {
            case "Start":
                Switchable = false;
                Visible = false;
                break;
            case "Main":
                Switchable = false;
                Visible = true;
                break;
            default:
                Switchable = true;
                Visible = false;
                break;
        }
        Update_Visibility();
    }

    // Checking for a key event
    private void Update ()
    {
        if(Switchable && Input.GetKeyDown(KeyCode.Escape))
        {
            if(Visible)
                Visible = false;
            else
                Visible = true;
            Update_Visibility();
        }
    }

    // Setting visibility 
    private void Update_Visibility ()
    {
        if(CanvasGroup)
        {
            if(Visible)
            {
                CanvasGroup.alpha = 0.8f;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                CanvasGroup.alpha = 0f;
                Cursor.lockState = CursorLockMode.Locked;
            }
            CanvasGroup.blocksRaycasts = Visible;
            CanvasGroup.interactable = Visible;
        }
    }

    // Makes menu non-interactable
    public void Disable_Menu ()
    {
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.interactable = false;
    }

    // Loading all keybinds
    private void Load_Keybinds ()
    {
        var file = Resources.Load<TextAsset>(Resource);
        if(file != null)
        {
            string json = file.text;
            Keybinds = JsonHelper.FromJson<Keybind>(json);
            Console.Log(this, "Found " + Keybinds.Length.ToString() + " keybinds in \"" + Resource + "\"");
        }
        else
            Console.Error(this, "Resource \"" + Resource + "\" doesn't exist");
    }

    // Loads all bindable methods into a dictionary
    private void Load_Bindable ()
    {
        var scripts = Player_Prefab.GetComponents<MonoBehaviour>(); // Loading all script components on the current character
        foreach(var script in scripts) // Looping through each script in array
        {
            string script_name = script.GetType().ToString().Split('_')[1]; // Getting script name suffix
            foreach(var method in script.GetType().GetMethods()) // Getting public bindable methods
                if((method.IsPublic && method.DeclaringType == script.GetType()) && (method.GetCustomAttributes(typeof(Bindable), false).Length > 0)) // Checking if the method was declared in the current script
                    Bindable.Add(script_name + "_" + method.Name, method);
        }
        Console.Log(this, "Found " + Bindable.Count.ToString() + " bindable methods");
    }

}
