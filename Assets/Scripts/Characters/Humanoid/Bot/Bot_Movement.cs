using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

// Controlling the current item
public class Bot_Movement : Lockable_Script
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private NavMeshAgent NavMeshAgent; // Nav mesh agent component
    private Human_Health Human_Health; // Human healts script

    private void Awake () 
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Human_Health = GetComponent<Human_Health>();
    }


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private GameObject Current_Target = null;


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################

    private void FixedUpdate () => Target_An_Enemy();

    // Finding and going after a new enemy
    private void Target_An_Enemy ()
    {
        if(!Human_Health.Alive)
        {
            NavMeshAgent.enabled = false;
            return;
        }
        // Following the current target
        if(Current_Target != null)
        {
            //Console.Warning(this, Vector3.Distance(this.transform.position, Current_Target.transform.position).ToString());
            //if(Vector3.Distance(this.transform.position, Current_Target.transform.position) > 3f)
                Follow_Target();
        }
        else
        {
            // Finding new target
            var all_objects = FindObjectsOfType<GameObject>();
            var enemies = new List<GameObject>();
            foreach(GameObject o in all_objects)
                if(o.layer == 7 && o != this.gameObject && o.tag != this.gameObject.tag)
                    enemies.Add(o);
            if(enemies.Count == 0) {
                return;
            }
            var random = new Random();
            Current_Target = enemies[random.Next(enemies.Count)];
            Follow_Target();
        }
    }

    // Following target - both by nav mesh and by rotating the bot itself
    private void Follow_Target ()
    {
        NavMeshAgent.SetDestination(Current_Target.transform.position);
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            Quaternion.LookRotation((Current_Target.transform.position - transform.position).normalized), 
            Time.fixedDeltaTime * 100f
        );
    }

}
