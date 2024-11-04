using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSFX : MonoBehaviour
{
    public AudioSource audioSource;   // Reference to the AudioSource component
    public AudioClip buttonClickSFX;  // The sound clip to play when the button is clicked

    void Start()
    {
        // Get the Button component and add a listener for the click event
        Debug.Log("Button clicked. Attempting to play sound.");
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlayClickSound);
        }
    }

    public void PlayClickSound()
    {
        // Play the button click sound using the AudioSource
        if (audioSource != null && buttonClickSFX != null)
        {
            audioSource.PlayOneShot(buttonClickSFX);
        }
    }
}
