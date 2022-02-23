using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public Keybind[] Keybinds = null; // Array of keybinds
    private string Resource = "Config/Keybinds"; // Name of resource that stores keybinds


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

}
