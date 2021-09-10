using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour {
    public Text consoleText;
    private IEnumerator currentTextEnumerator;
    private bool running;

    public void UpdateText(string text, float waitTime, float fadeTime) {
        if(running) {
            StopCoroutine(currentTextEnumerator);
        }
        currentTextEnumerator = HandleTextDisplay(text, waitTime, fadeTime);
        StartCoroutine(currentTextEnumerator);
    }

    private IEnumerator HandleTextDisplay(string text, float waitTime, float fadeTime) {
        running = true;
        consoleText.text = text;
        Color opaque = new Color(1f, 1f, 1f, 1f);
        consoleText.color = opaque;
        yield return new WaitForSeconds(waitTime);
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime) {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(1f, 0f, t));
            consoleText.color = newColor;
            yield return null;
        }
        running = false;
    }
}
