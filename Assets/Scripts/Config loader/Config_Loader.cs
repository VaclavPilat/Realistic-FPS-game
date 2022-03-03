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
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Loading configuration
    public static T[] Load<T> (string filename)
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
                return JsonHelper.FromJson<T>(stream.ReadToEnd());
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
                return JsonHelper.FromJson<T>(json);
            }
            else
                Console.Error(null, Prefix + "Couldn't find config file or resource matching name '" + filename + "'.");
        }
        return null;
    }

    // Saving configuration to file
    public static void Save<T> (string filename, T[] data)
    {
        string filepath = Get_Filepath(filename);
        using (var stream = new StreamWriter(filepath, false))
        {
            stream.Write(JsonHelper.ToJson<T>(data, true));
        }
        Console.Log(null, Prefix + "Data should be saved in '" + filepath + "'");
    }

}