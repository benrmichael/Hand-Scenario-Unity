using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandleInteraction : MonoBehaviour
{

    [SerializeField] public GameObject tableHandle;

    private bool lockButtonPressed = false;
    private bool unlockButtonPressed = false;

    private void Start()
    {
        if (tableHandle == null)
        {
            Debug.Log("Table handle not set!");
        }
    }

    public void LockTable()
    {
        if (lockButtonPressed) return;

        lockButtonPressed = true;
        ToggleHandleInteraction(false);
        lockButtonPressed = false;
    }

    public void UnlockTable()
    {
        if (unlockButtonPressed) return;

        unlockButtonPressed = true;
        ToggleHandleInteraction(true);
        unlockButtonPressed = false;
    }

    void ToggleHandleInteraction(bool isActive)
    {
        tableHandle.SetActive(isActive);
    }

}
