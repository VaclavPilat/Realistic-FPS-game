using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// Class for locking MonoBehaviour scripts and not allowing user to perform actions
public class Lockable_Script : MonoBehaviour 
{
    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private bool Locked = false; // Is the script locked?


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Getter
    public bool Is_Locked () 
    {
        return Locked;
    }

    // Getter
    public bool Is_Unlocked () 
    {
        return !Locked;
    }

    // Locking script
    public void Lock () 
    {
        Locked = true;
    }

    // Unlocking script
    public void Unlock ()
    {
        Locked = false;
    }

    // Locking a script and unlocking it after a set time
    public void Lock_For (float seconds)
    {
        Lock();
        StartCoroutine(Unlock_After(seconds));
    }

    // Unlocking a script after a set amount of time
    private IEnumerator Unlock_After (float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Unlock();
    }

}