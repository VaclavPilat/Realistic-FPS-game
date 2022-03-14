using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Projectile_Bullet : MonoBehaviour
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private Sound_Manager Sound_Manager; // Sound manager
    private void Awake () => Sound_Manager = GetComponent<Sound_Manager>();


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    // Move after being spawned
    private void Start () 
    {
        //Console.Log(this, "SPAWN");
        StartCoroutine(Despawn_After_Time(3f));
    }

    // Despawning bullet after hit
    private void OnCollisionEnter (Collision collision)
    {
        int layer = collision.gameObject.layer;
        switch(layer)
        {
            case 3: // Ground layer
                break;
            case 8: // Character bodypart layer
                try
                {
                    collision.transform.root.GetComponent<Human_Health>().Cause_Damage(collision.collider, 1000f);
                }
                catch (Exception e) {}
                Console.Warning(this, "Character hit - " + collision.transform.name + " (" + collision.transform.root.name + ")");
                break;
            default:
                Console.Warning(this, "hit " + collision.gameObject.name + " = layer " + layer.ToString());
                break;
        }
        GameObject.Destroy(gameObject);
    }

    // Despawning bullet after some time
    private IEnumerator Despawn_After_Time (float seconds)
    {
        yield return new WaitForSeconds(seconds);
        //Console.Log(this, "DESPAWNED");
        GameObject.Destroy(gameObject);
    }

}
