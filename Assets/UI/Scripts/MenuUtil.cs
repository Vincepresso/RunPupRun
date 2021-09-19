using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUtil : MonoBehaviour {
    public GameManager gameManager;
    public AudioSource audioSource;
    public AudioClip clickSfx;
    public void OnClickResumeButton() {
        audioSource.PlayOneShot(clickSfx);
        gameManager.isPaused = false;
    }
    public void OnClickPlayButton() {
        audioSource.PlayOneShot(clickSfx);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void OnClickRetryButton() {
        audioSource.PlayOneShot(clickSfx);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnClickQuitButton() {
        audioSource.PlayOneShot(clickSfx);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
