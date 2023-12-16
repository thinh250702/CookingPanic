using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;

    public float minTimeBetweenFootsteps = 0.3f; // Minimum time between footstep sounds
    public float maxTimeBetweenFootsteps = 0.6f; // Maximum time between footstep sounds
    private float timeSinceLastFootstep; // Time since the last footstep sound

    private void Awake() {
        player = gameObject.GetComponent<Player>();
    }

    private void Update() {
        // Check if the player is walking
        if (player.IsWalking()) {
            // Check if enough time has passed to play the next footstep sound
            if (Time.time - timeSinceLastFootstep >= Random.Range(minTimeBetweenFootsteps, maxTimeBetweenFootsteps)) {
                // Play a random footstep sound from the array
                SoundManager.Instance.PlayFootstepsSound(player.transform.position);
                timeSinceLastFootstep = Time.time; // Update the time since the last footstep sound
            }
        }
    }
}
