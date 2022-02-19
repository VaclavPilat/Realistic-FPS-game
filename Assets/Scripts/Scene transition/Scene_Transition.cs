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

    private string Resource = "Config/Tips"; // Resource name (address)


    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    // Triggering amination after a scene is loaded
    private void Awake () => SceneManager.sceneLoaded += Scene_Loaded;

    // Actins after a new scene is loaded
    private void Scene_Loaded (Scene scene, LoadSceneMode mode)
    {
        Console.Log(this, "Loaded scene " + scene.name + " with mode " + mode.ToString());
        Animator.SetTrigger("This scene");
    }

    // Loading another scene
    public void Load (string scene) => StartCoroutine(Load_Scene(scene));
    private IEnumerator Load_Scene (string scene)
    {
        Console.Log(this, "Loading \"" + scene + "\" scene...");
        // Setting menu properties
        Menu_Variables.Disable_Menu();
        // Getting new tip
        New_Tip();
        // Showing the animation
        if(Animator == null)
            Console.Error(this, "\"Animator\" is null");
        else
            Animator.SetTrigger("New scene");
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(scene);
    }

    // Exitting the game
    public void Exit () => StartCoroutine(Exit_Game());
    private IEnumerator Exit_Game ()
    {
        Console.Log(this, "Exitting game...");
        // Setting menu properties
        Menu_Variables.Disable_Menu();
        // Getting new tip
        New_Tip();
        // Showing the animation
        if(Animator == null)
            Console.Error(this, "\"Animator\" is null");
        else
            Animator.SetTrigger("New scene");
        yield return new WaitForSecondsRealtime(1);
        Application.Quit();
    }

    // Displaying a random tip
    private void New_Tip ()
    {
        if(Tip_Label)
        {
            // Loading tips
            var file = Resources.Load<TextAsset>(Resource);
            if(file != null)
            {
                string[] tips = JsonHelper.FromJson<string>(file.text);
                // Getting a random tip
                Random random = new Random();
                string tip = "<b>Tip: </b><color=#dddddd>" + tips[random.Next(0, tips.Length)] + "</color>";
                // Showing the tip
                Tip_Label.GetComponent<Text>().text = tip;
            }
            else
                Console.Warning(this, "Resource \"" + Resource + "\" doesn't exist");
        }
    }

}
