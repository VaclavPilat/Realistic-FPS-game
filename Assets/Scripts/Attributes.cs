using System;

// Attribute for marking bindable methods
public class Bindable : System.Attribute 
{
    public string Description;
    public float Time;

    public Bindable (string description = null, float time = 0f)
    {
        this.Description = description;
        this.Time = time;
    }
}