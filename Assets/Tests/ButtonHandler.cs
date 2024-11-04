using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip buttonSound;   // The sound clip to play

    void Start()
    {
        // Get the Button component from Unity's UI library and add a listener for the click event
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlaySound);
        }
    }

    public void PlaySound()
    {
        // Play the sound attached to the AudioSource
        audioSource.PlayOneShot(buttonSound);
    }
}
