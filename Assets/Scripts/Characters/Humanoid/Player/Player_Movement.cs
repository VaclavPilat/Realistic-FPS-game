using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_Movement : Lockable_Script
{
    //##########################################################################################
    //######################################  COMPONENTS  ######################################
    //##########################################################################################

    private CharacterController CharacterController; // Character controller
    private Transform Ground_Check; // Ground check
    private LayerMask Ground; // Layer mask for ground
    private Human_Health Human_Health; // Human healts script

    private void Awake ()
    {
        CharacterController = GetComponent<CharacterController>();
        Ground_Check = transform.Find("Ground Check");
        Ground = LayerMask.GetMask("Ground");
        Human_Health = GetComponent<Human_Health>();
    }


    //##########################################################################################
    //#############################  PRIVATE METHODS / VARIABLES  ##############################
    //##########################################################################################

    private Vector3 Last_Position = Vector3.zero;

    private float Speed_Maximum = 10f;
    private float Speed_Running;
    private float Speed_Forward;
    private float Speed_Backward;
    private float Speed_Sides;
    private float Speed_Jump;

    private bool Input_Forward  = false;
    private bool Input_Backward = false;
    private bool Input_Left     = false;
    private bool Input_Right    = false;
    private bool Input_Jump     = false;

    private bool Grounded = false;
    private float Gravity = 0f;

    private void FixedUpdate () 
    {
        if(Human_Health.Alive)
        {
            Update_Speed();
            Move();
        }
    }


    //##########################################################################################
    //##################################  PUBLIC VARIABLES  ####################################
    //##########################################################################################

    [HideInInspector] public Vector3 Position = Vector3.zero;
    [HideInInspector] public Vector3 Velocity = Vector3.zero;


    //##########################################################################################
    //###############################  PUBLIC BINDABLE METHODS  ################################
    //##########################################################################################

    [Bindable("Moves character forward")] 
    public bool Forward ()
    {
        Input_Forward = true;
        return true;
    }

    [Bindable("Moves character backwards")] 
    public bool Backward ()
    {
        Input_Backward = true;
        return true;
    }

    [Bindable("Moves character to the left")] 
    public bool Left ()
    {
        Input_Left = true;
        return true;
    }

    [Bindable("Moves character to the right")] 
    public bool Right ()
    {
        Input_Right = true;
        return true;
    }

    [Bindable("Jumps with the character")] 
    public bool Jump ()
    {
        Input_Jump = true;
        return false;
    }


    //##########################################################################################
    //#####################################  SCRIPT FLOW  ######################################
    //##########################################################################################
    
    // Calculating speed from two vectors (disabling strafing)
    private float Speed_Magnitude (float first, float second)
    {
        return (first + second) / 2f / (float)Math.Sqrt(2);
    }

    // Updating speed values
    private void Update_Speed ()
    {
        Speed_Running  = Speed_Maximum;
        Speed_Forward  = 0.5f * Speed_Running;
        Speed_Backward = 0.5f * Speed_Forward;
        Speed_Sides    = 0.6f * Speed_Forward;
    }

    // Updating variables on start of FixedUpdate
    private void Update_Variables_Start ()
    {
        Position = transform.position;
        Velocity = (Last_Position - Position) / Time.fixedDeltaTime;
        // Checking if opposite keys are pressed at once
        if (Input_Forward && Input_Backward)
            Input_Forward = Input_Backward = false;
        if (Input_Left && Input_Right)
            Input_Left = Input_Right = false;
        // Gravity
        Grounded = Physics.CheckSphere(Ground_Check.position, 0.2f, Ground);
    }
    
    // Updating variables at the end of FixedUpdate
    private void Update_Variables_End ()
    {
        Last_Position = Position;
        Input_Forward = Input_Backward = Input_Left = Input_Right = Input_Jump = false;
    }

    // Allows moving the character
    private void Move ()
    {
        Update_Variables_Start();
        Vector3 speed = Calculate_Speed();
        CharacterController.Move((
        //CharacterController.SimpleMove(
            transform.forward * speed.x +
            transform.right * speed.z +
            Vector3.up * speed.y
        ) * Time.fixedDeltaTime);
        //);
        Update_Variables_End();
    }

    // Applies speed to the character based on input
    private Vector3 Calculate_Speed ()
    {
        Vector3 speed = Vector3.zero;
        // Forward
        if(Input_Forward)
            if(Input_Left || Input_Right) // Forward + left or right
            {
                float magnitude = Speed_Magnitude(Speed_Forward, Speed_Sides);
                speed.x = magnitude;
                if(Input_Left)
                    speed.z = -magnitude;
                if(Input_Right)
                    speed.z = magnitude;
            }
            else // Forward only
                speed.x = Speed_Forward;
        // Backward
        if(Input_Backward)
            if(Input_Left || Input_Right) // Backward + left or right
            {
                float magnitude = Speed_Magnitude(Speed_Backward, Speed_Sides);
                speed.x = -magnitude;
                if(Input_Left)
                    speed.z = -magnitude;
                if(Input_Right)
                    speed.z = magnitude;
            }
            else // Backward only
                speed.x = -Speed_Backward;
        // Left
        if(Input_Left && !Input_Forward && !Input_Backward)
            speed.z = -Speed_Sides;
        // Right
        if(Input_Right && !Input_Forward && !Input_Backward)
            speed.z = Speed_Sides;
        // Gravity
        if(Grounded)
            speed.y = Gravity = -2f;
        else
            speed.y = Gravity = Gravity - 9.81f * Time.fixedDeltaTime;
        return speed;
    }


    //##########################################################################################
    //##################################  DEBUGGING TOSTRING  ##################################
    //##########################################################################################

    public override string ToString ()
    {
        string output = "";
        output += "Position: " + Position.ToString() + " m\n";
        output += "Velocity: " + Velocity.ToString() + ", " + Math.Round(Velocity.magnitude, 1).ToString() + " (" + Math.Round(new Vector2(Velocity.x, Velocity.z).magnitude, 1) + ") m/s\n";
        output += "Max speed: " + Speed_Maximum.ToString() + " m\n";
        output += "Grounded: " + Grounded.ToString() + "\n";
        return output;
    }

}
