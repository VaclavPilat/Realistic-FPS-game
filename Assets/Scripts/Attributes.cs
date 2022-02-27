using System;

// Attribute for marking bindable methods
public class Bindable : System.Attribute 
{
    public string Description; // Description of the binable method
    public float Time; // How much time (in seconds) it takes to perform this action

    public Bindable (string description = null, float time = 0f)
    {
        this.Description = description;
        this.Time = time;
    }
}