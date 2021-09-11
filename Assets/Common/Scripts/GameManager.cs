using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject baobaogoObj;
    public GameObject meroObj;
    private Baobaogo baobaogo;
    private Mero mero;
    public CinemachineVirtualCamera cmBaobaogo;
    public CinemachineVirtualCamera cmMero;
    public float cameraSwitchTime;
    public float delayBeforeRunningTime;
    public float meroForwardDistance;
    public float meroBackwardDistance;
    public float meroTeleportXDistance;
    public float meroTeleportYDistance;
    public Console consoleUI;
    public Text scoreText;
    private float scoreValue;
    public static GameManager current;
    private void Awake() {
        current = this;
    }
    void Start() {
        ActorEvents.current.onCliffEnter += ActorDies;
        ActorEvents.current.onMeroTouch += ActorHitByMero;
        StartCoroutine(ChangeCamera());
        baobaogo = baobaogoObj.GetComponent<Baobaogo>();
        mero = meroObj.GetComponent<Mero>();
        scoreValue = 0f;
    }
    void Update() {
        // This is to Clamp Mero's position relative to Baobaogo
        if(meroObj.transform.position.x > baobaogoObj.transform.position.x + meroForwardDistance || meroObj.transform.position.x < baobaogoObj.transform.position.x - meroBackwardDistance) {
            meroObj.transform.position = new Vector3(baobaogoObj.transform.position.x - meroTeleportXDistance, baobaogoObj.transform.position.y + meroTeleportYDistance, meroObj.transform.position.z);
        }
        if(baobaogo.gameBegin == true) {
            scoreValue += Time.deltaTime;
        }
        int scoreValueInSeconds = (int) scoreValue % 60;
        scoreText.text = scoreValueInSeconds.ToString();
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
        baobaogo.GetComponent<Baobaogo>().gameBegin = true;
        consoleUI.UpdateText("Press Space, Left Mouse, or Tap to Jump. That's all", 7f, 3f);
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
    private void ActorDies(GameObject actor) {
        Debug.Log(actor.name + " pass out!");
        Time.timeScale = 0;
    }
    void OnDestroy() {
        ActorEvents.current.onCliffEnter -= ActorDies;
        ActorEvents.current.onMeroTouch -= ActorHitByMero;
    }
}
