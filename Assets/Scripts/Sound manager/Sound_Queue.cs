using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

// Sound manager
public class Sound_Queue : Sound_Manager
{
    //##########################################################################################
    //###################################### SCRIPT FLOW #######################################
    //##########################################################################################

    private void Start () => StartCoroutine(Play());

    private IEnumerator Play ()
    {
        int i = 0;
        while(Sounds.Length > 0)
        {
            Sounds[i].Source.Play();
            yield return new WaitForSeconds(Sounds[i].Source.clip.length);
            if(i == Sounds.Length -1)
                i = 0;
            else
                i++;
        }
    }

}
