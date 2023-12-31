using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] public AudioSource ambientSoundtrack;
    [SerializeField] public GameObject AudioSlider;

    private Slider slider;
    private bool missingObjects = false;

    // Start is called before the first frame update
    void Start()
    {
        if (ambientSoundtrack == null || AudioSlider == null)
        {
            Debug.LogError("AudioSource or slider not assigned! audio change won't work.");
            missingObjects = true;
        }
        else
        {
            slider = AudioSlider.GetComponent<Slider>();
            if (slider == null)
            {
                Debug.Log("Unable to find slider component for AudioSlider game object!");
                missingObjects = true;
            }
        }
    }

    // Function to be called when the slider value changes
    public void ChangeVolume()
    {
        if (missingObjects) return;

        // Set the new volume: only allow the ambient music to be at max 0.5
        ambientSoundtrack.volume = slider.value * 0.5f;
    }
}
