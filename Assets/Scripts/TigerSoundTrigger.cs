using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerSoundTrigger : MonoBehaviour
{
    // Public variables for configuration
    public string playerTag = "Player"; // Tag to identify the Player
    public float triggerDistance = 10f; // Distance within which sound should play
    public AudioClip tigerSound; // The sound clip to play

    private AudioSource audioSource; // Audio source component
    private GameObject player; // Reference to the player object

    void Start()
    {
        // Find and cache the player GameObject by tag
        player = GameObject.FindGameObjectWithTag(playerTag);
        
        // Add or get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure the audio source
        audioSource.playOnAwake = false; // Don't play at start
        audioSource.loop = true;         // Keep the sound looping
        audioSource.spatialBlend = 1.0f; // Full 3D sound
        audioSource.clip = tigerSound;   // Assign the audio clip
    }

    void Update()
    {
        if (player == null) return; // Exit if player is not found

        // Calculate distance between the Tiger and the Player
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // Check if the player is within the trigger distance
        if (distance <= triggerDistance)
        {
            // If the sound isn't playing, play it
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // Stop the sound if the player is too far
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
