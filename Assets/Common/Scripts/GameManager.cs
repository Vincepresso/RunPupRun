using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Baobaogo baobaogo;
    public Mero mero;
    public CinemachineVirtualCamera cmBaobaogo;
    public CinemachineVirtualCamera cmMero;
    public float cameraSwitchTime;
    public float delayBeforeRunningTime;
    public float meroForwardDistance;
    public float meroBackwardDistance;
    public float meroTeleportXDistance;
    public float meroTeleportYDistance;
    public GameObject consoleUI;
    public GameObject scoreUI;
    public GameObject pauseUI;
    public GameObject gameOverUI;
    public float gameOverTime;
    private Text scoreText;
    private float scoreValue;
    public bool isPaused;
    public static GameManager current;
    private void Awake() {
        current = this;
    }
    void Start() {
        Time.timeScale = 1f;
        ActorEvents.current.onCliffEnter += ActorFellFromCliff;
        ActorEvents.current.onMeroTouch += ActorHitByMero;
        StartCoroutine(ChangeCamera());
        scoreText = scoreUI.transform.GetChild(0).GetComponent<Text>();
        scoreValue = 0f;
        scoreText.enabled = false;
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        isPaused = false;
    }
    void Update() {
        // This is to Clamp Mero's position relative to Baobaogo
        if(mero.transform.position.x > baobaogo.transform.position.x + meroForwardDistance || mero.transform.position.x < baobaogo.transform.position.x - meroBackwardDistance) {
            mero.transform.position = new Vector3(baobaogo.transform.position.x - meroTeleportXDistance, baobaogo.transform.position.y + meroTeleportYDistance, mero.transform.position.z);
        }
        if(baobaogo.gameBegin == true && !baobaogo.gameOver) {
            scoreText.enabled = true;
            scoreValue += Time.deltaTime;
            scoreText.text = scoreValue.ToString("n1");
        }
        if(!baobaogo.passOut) {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                isPaused = !isPaused;
            }
            if(isPaused) {
                Time.timeScale = 0f;
                baobaogo.gamePaused = true;
                pauseUI.SetActive(true);
            } else {
                pauseUI.SetActive(false);
                baobaogo.gamePaused = false;
                Time.timeScale = 1f;
            }
        }
    }
    private IEnumerator ChangeCamera() {
        Debug.Log("Panning camera to Mero");
        cmMero.Priority = 1;
        cmBaobaogo.Priority = 0;
        yield return new WaitForSeconds(cameraSwitchTime);
        Debug.Log("Panning camera to Baobaogo");
        cmMero.Priority = 0;
        cmBaobaogo.Priority = 1;
        yield return new WaitForSeconds(delayBeforeRunningTime);
        baobaogo.gameBegin = true;
        consoleUI.GetComponent<Console>().UpdateText("Press Space, Left Mouse, or Tap to Jump. That's all", 7f, 3f);
    }
    private void ActorHitByMero(GameObject actor) {
        if(actor.CompareTag("Player")) {
            if(baobaogo.isInvincible) {
                mero.Stagger();
                baobaogo.BounceForward();
            } else {
                ActorDies(actor);
            }
        }
    }
    private void ActorFellFromCliff(GameObject actor) {
        if(actor.CompareTag("Player")) {
            baobaogo.GetComponent<Rigidbody2D>().gravityScale = 0f;
            ActorDies(actor);
        }
    }
    private void ActorDies(GameObject actor) {
        Debug.Log(actor.name + " pass out!");
        baobaogo.passOut = true;
        GameOver();
    }
    void OnDestroy() {
        ActorEvents.current.onCliffEnter -= ActorFellFromCliff;
        ActorEvents.current.onMeroTouch -= ActorHitByMero;
    }
    private void GameOver() {
        StartCoroutine(GameOverCoroutine());
        mero.gameOver = true;
        baobaogo.gameOver = true;
    }
    private IEnumerator GameOverCoroutine() {
        yield return new WaitForSeconds(gameOverTime);
        consoleUI.SetActive(false);
        scoreUI.SetActive(false);
        pauseUI.SetActive(false);
        isPaused = false;
        gameOverUI.transform.GetChild(1).GetComponent<Text>().text = "Your score is " + scoreText.text;
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
