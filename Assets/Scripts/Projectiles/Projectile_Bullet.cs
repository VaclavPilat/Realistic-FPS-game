using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Console.Log(this, "SPAWN");
        StartCoroutine(Despawn_After_Time(3f));
    }

    // Despawning bullet after hit
    private void OnCollisionEnter (Collision collision)
    {
        Console.Log(this, "HIT " + collision.transform.name);
        GameObject.Destroy(gameObject);
    }

    // Despawning bullet after some time
    private IEnumerator Despawn_After_Time (float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Console.Log(this, "DESPAWNED");
        GameObject.Destroy(gameObject);
    }

}
