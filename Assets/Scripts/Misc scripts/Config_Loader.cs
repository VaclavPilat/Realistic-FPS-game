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
    public static bool Load (string filename)
    {
        try
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
                    try 
                    {
                        var stream = new StreamReader(filepath);
                        Config[filename] = JsonHelper.FromJson<Setting>(stream.ReadToEnd());
                        Generate(filename);
                        return true;
                    }
                    catch (Exception)
                    {
                        Console.Warning(null, "Config file '" + filename + "' is not valid, using resource variant instead.");
                    }
                }
                // If the config file doesn't exist, a resource file is used instead
                var file = Resources.Load<TextAsset>(resource);
                if(file != null)
                {
                    Console.Log(null, Prefix + "Resource file found: '" + resource + "'");
                    string json = file.text;
                    Config[filename] = JsonHelper.FromJson<Setting>(json);
                    Generate(filename);
                    return true;
                }
                else
                    Console.Warning(null, Prefix + "Couldn't find config file or resource matching name '" + filename + "'.");
            }
            else
                return true;
        }
        catch (Exception e)
        {
            Console.Error(null, "Cannot load config '" + filename + "': " + e.ToString());
        }
        return false;
    }

    // Generating setting contents if necessary
    private static void Generate (string filename)
    {
        foreach(Setting setting in Config[filename])
        {
            // Generating check values
            if(setting.Check.EndsWith(";"))
            {
                string check = Code_Compiler.Line<string>(setting.Check);
                setting.Check = check;
                setting.Value = (check.Split('|').Length -1).ToString();
                Save(filename); 
            }
        }
    }

    // Saving configuration to file
    public static bool Save (string filename)
    {
        try
        {
            string filepath = Get_Filepath(filename);
            using (var stream = new StreamWriter(filepath, false))
            {
                stream.Write(JsonHelper.ToJson<Setting>(Config[filename], true));
            }
            Console.Log(null, Prefix + "Data should be saved in '" + filepath + "'");
            return true;
        }
        catch (Exception e)
        {
            Console.Error(null, "Cannot save config '" + filename + "': " + e.ToString());
        }
        return false;
    }

    // Saving raw configuration to file
    public static bool Save_Raw (string filename, Setting[] content)
    {
        Config[filename] = content;
        return Save(filename);
    }

    // Getting a certain setting by name
    public static Setting Get (string filename, string settingname)
    {
        Load(filename);
        return Array.Find(Config_Loader.Config[filename], s => s.Name == settingname);
    }

}