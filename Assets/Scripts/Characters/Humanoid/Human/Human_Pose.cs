using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Setting pose
public class Human_Pose : Lockable_Script
{
    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private Character_Pose Pose = Character_Pose.Standing; // Character pose


    //##########################################################################################
    //###############################  PUBLIC BINDABLE METHODS  ################################
    //##########################################################################################

    [Bindable("Crouching")]
    public bool Crouch () 
    {
        if(Pose == Character_Pose.Crouching)
        {
            Pose = Character_Pose.Standing;
            return false;
        }
        else
        {
            Pose = Character_Pose.Crouching;
            return true;
        }
    }

    [Bindable("Lying down")]
    public bool Lie () 
    {
        if(Pose == Character_Pose.Lying)
        {
            Pose = Character_Pose.Crouching;
            return false;
        }
        else
        {
            Pose = Character_Pose.Lying;
            return true;
        }
    }

    [Bindable("Standing up")]
    public bool Standup () 
    {
        if(Pose != Character_Pose.Standing)
        {
            Pose = Character_Pose.Standing;
            return true;
        }
        else
            return false;
    }


    //##########################################################################################
    //##################################  DEBUGGING TOSTRING  ##################################
    //##########################################################################################

    public override string ToString ()
    {
        string output = "";
        output += "Pose: " + Pose.ToString() + "\n";
        return output;
    }

}
