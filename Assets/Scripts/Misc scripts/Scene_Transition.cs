using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class Scene_Transition : MonoBehaviour
{
    //##########################################################################################
    //####################################### COMPONENTS #######################################
    //##########################################################################################

    [SerializeField] private Animator Animator; // Animator component for transitions
    [SerializeField] private GameObject Tip_Label; // Label for displaying a random tip
    [SerializeField] private Menu_Variables Menu_Variables; // Main menu script


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private bool Loading_Scene = false; // Is a scene being loaded?
    private string Resource = "Config/Tips"; // Name of a resource that contains screen tips
    private string[] Tips = null; // Array with screen tips


    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    // Triggering amination after a scene is loaded
    private void Awake () 
    {
        Load_Tips();
        Tip_Label.SetActive(false);
        SceneManager.sceneLoaded += Scene_Loaded;
    }

    // Loading tips
    private void Load_Tips ()
    {
        var file = Resources.Load<TextAsset>(Resource);
        if(file != null)
        {
            Tips = JsonHelper.FromJson<string>(file.text);
            Console.Log(this, "Found " + Tips.Length.ToString() + " tips in \"" + Resource + "\".");
        }
        else
            Console.Warning(this, "Resource \"" + Resource + "\" doesn't exist");
    }

    // Actins after a new scene is loaded
    private void Scene_Loaded (Scene scene, LoadSceneMode mode)
    {
        Loading_Scene = false;
        Console.Log(this, "Loaded scene " + scene.name + " with mode " + mode.ToString());
        Animator.SetTrigger("This scene");
    }

    // Loading another scene
    public void Load (string scene) => StartCoroutine(Load_Scene(scene));
    private IEnumerator Load_Scene (string scene)
    {
        // Disabling calls from Main to Main
        if(SceneManager.GetActiveScene().name == "Main" && scene == "Main")
        {
            yield return null;
        }
        else
        {
            Console.Log(this, "Loading \"" + scene + "\" scene...");
            yield return Transition();
            // Doing stuff while the scene is still loading
            AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
            while(!operation.isDone)
            {
                //Console.Log(this, operation.progress.ToString());
                yield return null;
            }
        }
    }

    // Exitting the game
    public void Exit () => StartCoroutine(Exit_Game());
    private IEnumerator Exit_Game ()
    {
        Console.Log(this, "Exitting game...");
        yield return Transition();
        Application.Quit();
    }

    // Performing transition
    private IEnumerator Transition ()
    {
        // Setting screen tips
        Tip_Label.SetActive(true);
        // Setting menu properties
        Menu_Variables.Disable_Menu();
        // Getting new tip
        StartCoroutine(Change_Tips());
        // Showing the animation
        if(Animator == null)
            Console.Error(this, "\"Animator\" is null");
        else
            Animator.SetTrigger("New scene");
        yield return new WaitForSecondsRealtime(1);
    }

    // Displaying a random tip
    private void New_Tip ()
    {
        if(Tip_Label && Tips != null && Tips.Length > 0)
        {
            // Getting a random tip
            Random random = new Random();
            string tip = "<b>Tip: </b><color=#dddddd>" + Tips[random.Next(0, Tips.Length)] + "</color>";
            // Showing the tip
            Tip_Label.GetComponent<Text>().text = tip;
        }
    }

    // Changing tips after a while
    private IEnumerator Change_Tips ()
    {
        Loading_Scene = true;
        while(Loading_Scene)
        {
            New_Tip();
            Console.Log(this, "New tip shown");
            yield return new WaitForSecondsRealtime(5f);
        }
    }

}
