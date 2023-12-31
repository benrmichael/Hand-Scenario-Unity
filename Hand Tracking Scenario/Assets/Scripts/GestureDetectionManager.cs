using UnityEngine;
using TMPro;
using UnityEngine.UI;


// Manages the main loop of the scene
public class GestureDetectionManager : MonoBehaviour
{
    // All the gestures are mapped to an int:
    // right hand thumbs up = 0
    //
    //
    //
    //
    // right hand thumbs down = 1

    // The audio played in the beginning of the scene
    [SerializeField] public AudioSource introAudio;

    // The audio played to walk the user through the first gesture match
    [SerializeField] public AudioSource trialAudio;

    [SerializeField] public Text gestureDescription;
    [SerializeField] public Text mainButtonText;
    [SerializeField] public GameObject gestureParent;
    [SerializeField] public GameObject indicatorParent;
    [SerializeField] public Texture2D[] gestureImages;

    private RawImage displayedGesture;
    private Image indicator;
    private bool _introComplete = false;
    private bool _trialComplete = false;
    private bool _trialStarted = false;
    private bool _introInvoked = false;
    private bool _firstGestureGenerated = false;
    private bool _gestureMatchingStarted = false;
    private bool _buttonFunctionalityChanged = false;

    private int currentGesture;

    private void Start()
    {
        displayedGesture = gestureParent.GetComponent<RawImage>();
        indicator = indicatorParent.GetComponent<Image>();
        if (displayedGesture == null || indicator == null)
        {
            Debug.Log("Raw image or image comp not found!");
        }

        Invoke("PlayIntroduction", 3f);
    }

    private void PlayIntroduction()
    {
        _introInvoked = true;
        introAudio.Play();
    }

    void Update()
    {
        if (_introInvoked && !introAudio.isPlaying && introAudio.time == 0.0f)
        {
            _introComplete = true;
        }

        if (_trialStarted && _introComplete && !trialAudio.isPlaying && trialAudio.time == 0.0f)
        {
            _trialComplete = true;
            _gestureMatchingStarted = true;
            GenerateFirstGesture();
        }
    }

    private void GenerateFirstGesture()
    {
        if (_firstGestureGenerated) return;

        _firstGestureGenerated = true;
        GenerateGesture();
    }

    private void GenerateGesture()
    {
        currentGesture = Random.Range(0, gestureImages.Length);
        displayedGesture.texture = gestureImages[currentGesture];
        gestureDescription.text = gestureImages[currentGesture].name;
    }

    private void ChangeButtonFunctionality()
    {
        if (_buttonFunctionalityChanged) return;

        _buttonFunctionalityChanged = true;
        mainButtonText.text = "Pass";
    }

    // method manages the main button being pressed in the scene
    // it is not able to be pressed until the introduction audio is done playing, and once it is done the function of it changes
    public void MainButtonPressed()
    {
        if (!_introComplete)
        {
            return;
        }

        if(!_trialStarted)
        {
            _trialStarted = true;
            trialAudio.Play();
            ChangeButtonFunctionality();
        }

        if (!_trialComplete)
        {
            return;
        }
    }

    public void GestureMade(int gestureType)
    {
        if (!_gestureMatchingStarted) return;

        Debug.Log("Gesture made called! Gesture type = " + gestureType + ", the gesture we need is: " + currentGesture);

        int threshold = (currentGesture * 2) + 1;

        Debug.Log("The gesture threshold is " + threshold);
        if (gestureType == threshold || (gestureType + 1) == threshold)
        {
            indicator.color = Color.green;
            GenerateGesture();
        }
        else
        {
            indicator.color = Color.red;
        }
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
