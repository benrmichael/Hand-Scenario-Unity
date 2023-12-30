using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAndAnimationController : MonoBehaviour
{
    [SerializeField] public AudioSource introAudioClip;
    [SerializeField] public Animator animator;

    private bool readyToBegin = false;

    private int isWavingTriggerHash, isIdleHash, thumbsUpHash;

    void Start()
    {
        isWavingTriggerHash = Animator.StringToHash("isWaving");
        thumbsUpHash = Animator.StringToHash("thumbsUp");
        isIdleHash = Animator.StringToHash("isIdle");
        StartCoroutine(BufferIntro());
    }

    IEnumerator BufferIntro()
    {
        yield return new WaitForSeconds(4f);
        PlayIntro();
    }

    void PlayIntro()
    {
        // Set the trigger to start waving animation
        animator.SetTrigger(isWavingTriggerHash);
        // Play the intro audio clip
        introAudioClip.Play();
    }

    public void BeginButtonPressed()
    {
        Debug.Log("Begin button pressed!");
        if (readyToBegin)
        {
            Debug.Log("Begin button pressed! && Ready!");
            animator.SetTrigger(thumbsUpHash);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!introAudioClip.isPlaying && introAudioClip.time == 0.0f)
        {
            readyToBegin = true;
        }
    }
}

