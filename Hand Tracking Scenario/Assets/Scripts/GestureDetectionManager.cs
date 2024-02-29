using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// Manages the main loop of the scene
public class GestureDetectionManager : MonoBehaviour
{
    // All the gestures are mapped to an int:
    // Thumbs up = 0
    // Thumbs down = 1
    // Open palm up = 2
    // Fist bump = 3
    // Shaka = 4
    // Point at this = 5
    // Bunny = 6
    // Okay sign = 7
    // One finger up = 8

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
    private bool _gestureMatchingActive = false; 
    private bool _buttonFunctionalityChanged = false; // flag as to wether the the button is for beginning the trial or passing
    private bool _isColorChanging = false; // flag set to ignore gesure recognition when changing color (inidcate they got it wrong or right)
    private bool _isGestureChanging = false; // flag set to ignore gesure recognition when changing image

    private float fadeDuration = 1f; // controls how long it takes for images to fade in or out

    private int currentGesture;     // the current gesture index
    private int completedGestures = 0;
    private int numberOfMisses = 0; // to keep track of how many times they make an incorrect gesture

    struct Gesture {
        public int index;
        public int count;
        public int misses;

        public Gesture(int num) {
            index = num;
            count = 2;
            misses = 0;
        }

        public bool DecrementCount() {
            count--;
            Debug.Log("Count is now: " + count);
            return count <= 0;
        }
    }
    private List<Gesture> gestureList;

    private void Start()
    {
        displayedGesture = gestureParent.GetComponent<RawImage>();
        indicator = indicatorParent.GetComponent<Image>();
        if (displayedGesture == null || indicator == null)
        {
            Debug.Log("Raw image or image comp not found!");
        }

        Invoke("PlayIntroduction", 3f);
        FillGestureList();
    }

    private void PlayIntroduction()
    {
        _introInvoked = true;
        introAudio.Play();
    }

    // Used to create a map for each gesture and a count (the amount of times the player needs to match)
    private void FillGestureList()
    {
        gestureList = new List<Gesture>();
        for (int i = 0; i < gestureImages.Length; i++)
        {
            gestureList.Add(new Gesture(2));
        }
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
            _gestureMatchingActive = true;
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
        if (completedGestures == gestureImages.Length) {
            return;
        }

        currentGesture = Random.Range(0, gestureList.Count);
        bool done = gestureList[currentGesture].DecrementCount();
        if (done) {
            Debug.Log("Detected a done gesture, going to remove it now...");
            gestureList.RemoveAt(currentGesture);
            completedGestures += 1;
        }


        StartCoroutine(FadeOutAndChangeGesture(currentGesture));
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
        if (!_gestureMatchingActive || _isColorChanging || _isGestureChanging) return;

        Debug.Log("Gesture made called! Gesture type = " + gestureType + ", the gesture we need is: " + currentGesture);

        if (gestureType == currentGesture)
        {
            indicator.color = Color.green;
            if (completedGestures == gestureImages.Length)
            {
                _gestureMatchingActive = false;
                Debug.Log("All done with the gesture matching!");
                return;
            }
            StartCoroutine(WaitAndGenerateGesture(0.5f));
        }
        else
        {
            indicator.color = Color.red;
            numberOfMisses += 1;
            StartCoroutine(WaitAndChangeColor(1.5f));
        }
    }

    IEnumerator WaitAndGenerateGesture(float seconds)
    {
        _isColorChanging = true;
        yield return new WaitForSeconds(seconds);
        indicator.color = Color.white;
        _isColorChanging = false;
        GenerateGesture();
    }

    IEnumerator WaitAndChangeColor(float seconds)
    {
        _isColorChanging = true;
        yield return new WaitForSeconds(seconds);
        indicator.color = Color.white;
        _isColorChanging = false;
    }


    public void GestureEnded(int gestureType)
    {
    }

    // Coroutine to fade out the current image, change to the new image, and fade it in
    private IEnumerator FadeOutAndChangeGesture(int gestureIndex)
    {
        _isGestureChanging = true;
        float timer = 0f;
        Color startColor = displayedGesture.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            displayedGesture.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            yield return null;
        }

        displayedGesture.texture = gestureImages[gestureIndex];

        timer = 0f;
        startColor = new Color(displayedGesture.color.r, displayedGesture.color.g, displayedGesture.color.b, 0f);
        endColor = new Color(displayedGesture.color.r, displayedGesture.color.g, displayedGesture.color.b, 1f);

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            displayedGesture.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            yield return null;
        }
        _isGestureChanging = false;
    }
}
