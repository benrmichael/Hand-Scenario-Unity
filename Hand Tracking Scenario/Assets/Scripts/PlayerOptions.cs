using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerOptions : MonoBehaviour
{
    [SerializeField] public GameObject ConfirmOptions;

    private int optionIdentifier;

    private void Start()
    {
        if (ConfirmOptions == null)
        {
            Debug.Log("ConfirmOptions can't be null!");
        }
    }

    public void OptionButtonClicked(int identifier)
    {
        optionIdentifier = identifier;
        ConfirmOptions.SetActive(true);
    }

    public void YesButtonClicked()
    {
        if (optionIdentifier == 0)
        {
            Application.Quit();
        }
        else if (optionIdentifier == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (optionIdentifier == 2)
        {
            SceneManager.LoadScene("Main");
        }
    }

    public void NoButtonClicked()
    {
        ConfirmOptions.SetActive(false);
    }
}
