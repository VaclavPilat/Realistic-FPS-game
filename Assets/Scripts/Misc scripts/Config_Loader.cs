using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Class for loading configuration files
public static class Config_Loader
{
    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################
    
    private static string Prefix = "Config loader ::: "; // Console prefix message

    // Getting config file path
    private static string Get_Filepath (string filename)
    {
        return Application.persistentDataPath + "/" + filename + ".json";
    }

    // Getting reource path
    private static string Get_Resource (string filename)
    {
        return "Config/" + filename;
    }


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    public static Dictionary<string, Setting[]> Config = new Dictionary<string, Setting[]>(); // Storage for all configuration


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Loading configuration
    public static void Load (string filename)
    {
        if(!Config.ContainsKey(filename))
        {
            // Getting file paths
            string filepath = Get_Filepath(filename);
            string resource = Get_Resource(filename);
            // Checking if a configuration file already exists
            if(File.Exists(filepath))
            {
                Console.Log(null, Prefix + "Config file found: '" + filepath + "'");
                using (var stream = new StreamReader(filepath))
                {
                    Config[filename] = JsonHelper.FromJson<Setting>(stream.ReadToEnd());
                }
            }
            else
            // If the config file doesn't exist, a resource file is used instead
            {
                var file = Resources.Load<TextAsset>(resource);
                if(file != null)
                {
                    Console.Log(null, Prefix + "Resource file found: '" + resource + "'");
                    string json = file.text;
                    Config[filename] = JsonHelper.FromJson<Setting>(json);
                }
                else
                    Console.Error(null, Prefix + "Couldn't find config file or resource matching name '" + filename + "'.");
            }
        }
    }

    // Saving configuration to file
    public static void Save (string filename)
    {
        string filepath = Get_Filepath(filename);
        using (var stream = new StreamWriter(filepath, false))
        {
            stream.Write(JsonHelper.ToJson<Setting>(Config[filename], true));
        }
        Console.Log(null, Prefix + "Data should be saved in '" + filepath + "'");
    }

    // Getting a certain setting by name
    public static Setting Get (string filename, string settingname)
    {
        return Array.Find(Config_Loader.Config[filename], s => s.Name == settingname);
    }

}