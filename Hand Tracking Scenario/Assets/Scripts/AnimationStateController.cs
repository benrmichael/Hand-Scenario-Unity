using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{

    private Animator animator;

    private int isWavingHash;
    private int isIdleHash;
    
    // Start is called before the first frame update
    void Start()
    {
        isWavingHash = Animator.StringToHash("isWaving");
        isIdleHash = Animator.StringToHash("isIdle");
    }
}
