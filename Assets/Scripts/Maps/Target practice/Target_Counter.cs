using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Target_Counter : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    [SerializeField] private Text Count;
    [SerializeField] private Text Timer;


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private int Total = -1; // Total number of paper targets
    private int Destroyed = -1; // Number of destroyed paper targets
    private float Time_Since = -1f; // Time since the start of the app
    private float Current_Time = -1f; // Time spent on this map
    private bool Started = false; // Is the run started?
    private bool Ended = false; // Is the run ended?


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    private void Awake () => StartCoroutine(Check_Targets());

    // Checking target count
    private IEnumerator Check_Targets()
    {
        while(true)
        {
            // Getting all paper targets in scene
            GameObject[] all_objects = FindObjectsOfType<GameObject>();
            List<GameObject> paper_targets = new List<GameObject>();
            foreach(GameObject gameobject in all_objects)
                if (gameobject.layer == 9)
                    paper_targets.Add(gameobject);
            // Setting total number of targets
            if(Total < 0)
                Total = paper_targets.Count;
            Destroyed = Total - paper_targets.Count;
            if(Started && Time_Since < 0)
                Time_Since = Time.time;
            if(!Ended && paper_targets.Count != 0 && Time_Since >= 0)
                Current_Time = Time.time - Time_Since;
            if(!Ended && Destroyed == Total)
                End_Run();
            // Showing variables
            Count.text = Destroyed.ToString() + "/" + Total.ToString();
            Timer.text = (Current_Time < 0 ? "0.00" : Current_Time.ToString("0.00")) + "s";
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Starting the current run
    private void Start_Run ()
    {
        Started = true;
        Console.Log(this, "Run started");
    }

    // Ending the current run
    private void End_Run ()
    {
        Ended = true;
        Console.Log(this, "Run ended");
        // Creating record setting
        Setting setting = new Setting();
        setting.Type = "record";
        setting.Description = DateTime.Now.ToString("dd.MM.yyyy H:mm:ss");
        setting.Value = Current_Time.ToString();
        // Saving record settings
        string filename = SceneManager.GetActiveScene().name;
        Setting[] new_settings;
        if(Config_Loader.Load(filename))
        {
            // Creating new setting array
            new_settings = new Setting[Config_Loader.Config[filename].Length + 1];
            for(int i = 0; i < Config_Loader.Config[filename].Length; i++)
                new_settings[i] = Config_Loader.Config[filename][i];
            new_settings[new_settings.Length -1] = setting;
            Array.Sort(new_settings, delegate(Setting x, Setting y) 
            {
                return float.Parse(x.Value).CompareTo( float.Parse(y.Value) ); 
            });
            Config_Loader.Config[filename] = new_settings;
            Config_Loader.Save(filename);
        }
        else
        {
            new_settings = new Setting[1];
            new_settings[0] = setting;
            Config_Loader.Save_Raw(filename, new_settings);
        }
    }

    // Starting run after the player triggers the start collider
    private void OnTriggerEnter (Collider collider)
    {
        if(!Started && (collider.gameObject.layer == 7 || collider.gameObject.layer == 8))
            Start_Run();
    }
}
