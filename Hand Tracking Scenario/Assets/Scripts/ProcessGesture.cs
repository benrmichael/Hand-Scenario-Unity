using UnityEngine;

public class ProcessGesture : MonoBehaviour
{
    // All the gestures are mapped to an int:
    // right hand thumbs up = 0
    //
    //
    //
    //
    // right hand thumbs down = 1

    public void GestureMade(int gestureType)
    {
        
    }

    public void GestureEnded(int gestureType)
    {
    }

    public void StartButtonPressed()
    {
        Debug.Log("The button has been pressed!");
    }

    public void BunnyPerformed()
    {
        Debug.Log("Bunny performed!");
    }
}
