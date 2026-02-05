using UnityEngine;

public interface IToggleable
{
    //Multiple buttons can toggle the same object, this keeps track of how many
    int ToggleSources { get; set;}
    
    public void ToggleOn();
    public void ToggleOff();
}
