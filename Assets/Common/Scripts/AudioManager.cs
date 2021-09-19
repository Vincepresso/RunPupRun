using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager current;
    public Baobaogo baobaogo;
    public Mero mero;
    public float rollDistanceThreshold;
    public float rollMaxVolume;
    public float anxietyDistanceThreshold;
    public float anxietyMaxVolume;
    public float anxietyFadeOutTime;

    void Awake() {
        current  = this;
    }
    void Start() {
        mero.meroIsClosedAudioSource.Stop();
        mero.meroIsClosedAudioSource.volume = anxietyMaxVolume;
        mero.rollAudioSource.UnPause();
        mero.laughAudioSource.UnPause();
    }
    void Update() {
        ToggleMeroLaugh();
        ToggleMeroRoll();
        ToggleMeroIsClosed();
        AdjustMeroAudioVolume(mero.rollAudioSource, rollDistanceThreshold, rollMaxVolume);
    }
    public void AdjustMeroAudioVolume(AudioSource audioSource, float distanceThreshold, float maxVolume) {
        float distanceBetween = Mathf.Abs(baobaogo.transform.position.x - mero.transform.position.x);
        float lerp = maxVolume - (distanceBetween / distanceThreshold);
        audioSource.volume = lerp;
    }
    private void ToggleMeroIsClosed() {
        float distanceBetween = Mathf.Abs(baobaogo.transform.position.x - mero.transform.position.x);
        if(distanceBetween <= anxietyDistanceThreshold) {
            if(!mero.meroIsClosedAudioSource.isPlaying) {
                StopAllCoroutines();
                if(!baobaogo.gameOver) {
                    mero.meroIsClosedAudioSource.volume = anxietyMaxVolume;
                }
                mero.meroIsClosedAudioSource.Play();
            }
        } else {
            if(mero.meroIsClosedAudioSource.isPlaying) {
                StartCoroutine(AudioFadeOut(mero.meroIsClosedAudioSource, anxietyFadeOutTime));
            }
        }
        if(GameManager.current.isPaused) {
            mero.meroIsClosedAudioSource.Pause();
        } else {
            mero.meroIsClosedAudioSource.UnPause();
        }
        if(baobaogo.gameOver && mero.meroIsClosedAudioSource.isPlaying) {
            StartCoroutine(AudioFadeOut(mero.meroIsClosedAudioSource, anxietyFadeOutTime));
        }
    }
    private IEnumerator AudioFadeOut(AudioSource audioSource, float fadeTime) {
        float currentTime = 0f;
        float volumeStart = audioSource.volume;
        while(currentTime < fadeTime) {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(volumeStart, 0f, currentTime/fadeTime);
            yield return null;
        }
        audioSource.Stop();
    } 
    private void ToggleMeroRoll() {
        if(!GameManager.current.isPaused && baobaogo.gameBegin && !baobaogo.gameOver) {
            mero.rollAudioSource.UnPause();
        } else {
            mero.rollAudioSource.Pause();
        }
    }
    private void ToggleMeroLaugh() {
        if(GameManager.current.isPaused) {
            mero.laughAudioSource.Pause();
        } else {
            mero.laughAudioSource.UnPause();
        }
    }
}
