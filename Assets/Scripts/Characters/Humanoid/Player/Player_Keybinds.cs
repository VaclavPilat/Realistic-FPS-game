using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class Player_Keybinds : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private Human_Health Human_Health = null; // Player health for enabling checking keybinds
    private Menu_Variables Menu_Variables = null; // Menu variables (for getting keybinds and bindable methods)

    private void Awake () 
    {
        Human_Health = transform.GetComponent<Human_Health>();
        Menu_Variables = GameObject.Find("/Menu").GetComponent<Menu_Variables>();
    }


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private Dictionary<string, MethodInfo> Bindable_Methods = new Dictionary<string, MethodInfo>(); // Dictionary with all bindable methods this player can use
    private HashSet<string> Successful = new HashSet<string>(); // Successfully executed commands
    private HashSet<string> Unsuccessful = new HashSet<string>(); // Unsuccessfully executed commands
    private HashSet<string> Incorrect = new HashSet<string>(); // Incorrectly executed commands

    private void Start ()
    {
        Load_Bindable(); // Loads all bindable methods into a dictionary
        StartCoroutine(Clear_Debug(1f)); // Clears debug messages each X seconds
    }

    private void FixedUpdate () => Check_Keybinds(); // Checking for actions on each frame


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Regularly clearing debug messages
    private IEnumerator Clear_Debug (float interval)
    {
        while(Human_Health.Alive)
        {
            yield return new WaitForSeconds(interval);
            Successful.Clear();
            Unsuccessful.Clear();
            Incorrect.Clear();
        }
    }

    // Loads all bindable methods into a dictionary
    private void Load_Bindable ()
    {
        var scripts = transform.GetComponents<MonoBehaviour>(); // Loading all script components on the current character
        foreach(var script in scripts) // Looping through each script in array
        {
            string script_name = script.GetType().ToString().Split('_')[1]; // Getting script name suffix
            foreach(var method in script.GetType().GetMethods()) // Getting public bindable methods
                if((method.IsPublic && method.DeclaringType == script.GetType()) && (method.GetCustomAttributes(typeof(Bindable), false).Length > 0)) // Checking if the method was declared in the current script
                    Bindable_Methods.Add(script_name + "_" + method.Name, method);
        }
        Console.Log(this, "Found " + Bindable_Methods.Count.ToString() + " bindable methods");
    }

    // Checking if any keys hve been pressed
    private void Check_Keybinds ()
    {
        foreach (Keybind keybind in Menu_Variables.Keybinds)
        {
            if(Input.GetKeyDown(keybind.KeyCode) && (keybind.KeyDown != null) && (keybind.KeyDown != ""))
                StartCoroutine(Call_Commands(keybind.KeyDown));
            if(Input.GetKey(keybind.KeyCode)     && (keybind.Key != null)     && (keybind.Key != ""))
                StartCoroutine(Call_Commands(keybind.Key));
            if(Input.GetKeyUp(keybind.KeyCode)   && (keybind.KeyUp != null)   && (keybind.KeyUp != ""))
                StartCoroutine(Call_Commands(keybind.KeyUp));
        }
    }

    // Getting command list from tring and calling them
    private IEnumerator Call_Commands (string commands)
    {
        string[] commands_split = commands.Split(';');
        foreach(string command_cluster in commands_split)
            foreach(string command in command_cluster.Split('&'))
            {
                MethodInfo method = Find_Method(command.Trim());
                if(method != null)
                {
                    // Calling command
                    if ((bool) method.Invoke(transform.GetComponent(method.DeclaringType), null))
                        Successful.Add(command);
                    else
                    {
                        Unsuccessful.Add(command);
                        break;
                    }
                    // Waiting for a specified amount of time
                    Bindable attribute = (Bindable) method.GetCustomAttributes(typeof(Bindable), false)[0];
                    yield return new WaitForSeconds(attribute.Time);
                }
                else
                {
                    Incorrect.Add(command);
                    break;
                }
            }
    }

    // Attempts to find a method that corresponds to the selected command
    private MethodInfo Find_Method (string command_name)
    {
        if(Bindable_Methods.ContainsKey(command_name))
            return Bindable_Methods[command_name];
        else
            return null;
    }


    //##########################################################################################
    //##################################  DEBUGGING TOSTRING  ##################################
    //##########################################################################################

    public override string ToString ()
    {
        string output = "";
        output += "Successful commands: ";
        foreach(string command in Successful)
            output += command + ", ";
        output += "\n";
        output += "Unsuccessful commands: ";
        foreach(string command in Unsuccessful)
            output += command + ", ";
        output += "\n";
        output += "Incorrect commands: ";
        foreach(string command in Incorrect)
            output += command + ", ";
        output += "\n";
        return output;
    }

}
