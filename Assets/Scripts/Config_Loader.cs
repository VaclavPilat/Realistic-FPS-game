using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Class for loading configuration files
public static class Config_Loader
{
    private static string Prefix = "Config loader ::: ";

    // Loading configuration
    public static T[] Load<T> (string filename)
    {
        // Getting file paths
        string filepath = Application.persistentDataPath + "/" + filename + ".json";
        string resource = "Config/" + filename;
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

}