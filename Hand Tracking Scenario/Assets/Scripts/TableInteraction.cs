using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TableInteraction : MonoBehaviour
{

    [SerializeField] public GameObject tableHandle;
    [SerializeField] public GameObject playerSettingsUI;

    private bool lockButtonPressed = false;
    private bool unlockButtonPressed = false;

    private void Start()
    {
        if (tableHandle == null)
        {
            Debug.Log("Table handle not set!");
        }
    }

    // Method called from the lock table button
    public void LockTable()
    {
        if (lockButtonPressed) return;

        lockButtonPressed = true;
        ToggleHandleInteraction(false);
        lockButtonPressed = false;
    }

    // Method called from the unlock table button
    public void UnlockTable()
    {
        if (unlockButtonPressed) return;

        unlockButtonPressed = true;
        ToggleHandleInteraction(true);
        unlockButtonPressed = false;
    }

    // Change the active state of the table hanlde
    void ToggleHandleInteraction(bool isActive)
    {
        tableHandle.SetActive(isActive);
    }

    // Method called when the toggle settings button is pressed to set the settings UI to active or inactive
    public void TogglePlayerSettingsUI()
    {
        playerSettingsUI.SetActive(!playerSettingsUI.activeInHierarchy);
    }
}
