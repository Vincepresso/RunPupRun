using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaobaogoSfx : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip footstepsSfx;
    public AudioClip jumpSfx;
    public AudioClip highJumpSfx;
    public AudioClip itemPickup;
    public void PlayFootstepsSfx() {
        audioSource.PlayOneShot(footstepsSfx);
    }
    public void PlayJumpSfx() {
        audioSource.PlayOneShot(jumpSfx);
    }
    public void PlayHighJumpSfx() {
        audioSource.PlayOneShot(highJumpSfx);
    }
    public void PlayItemPickupSfx() {
        audioSource.PlayOneShot(itemPickup);
    }
}
