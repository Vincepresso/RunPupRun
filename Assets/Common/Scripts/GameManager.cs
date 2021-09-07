using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour {

    public GameObject baobaogo;
    public GameObject mero;
    public CinemachineVirtualCamera cmBaobaogo;
    public CinemachineVirtualCamera cmMero;
    public float cameraSwitchTime;

    public static GameManager current;
    private void Awake() {
        current = this;
    }
    void Start() {
        ActorEvents.current.onCliffEnter += ActorDies;
        ActorEvents.current.onMeroTouch += ActorDies;
        StartCoroutine(ChangeCamera());
    }
    private IEnumerator ChangeCamera() {
        Debug.Log("Panning camera to Mero");
        cmMero.Priority = 1;
        cmBaobaogo.Priority = 0;
        yield return new WaitForSeconds(cameraSwitchTime);
        Debug.Log("Panning camera to Baobaogo");
        cmMero.Priority = 0;
        cmBaobaogo.Priority = 1;
    }
    private void ActorDies(GameObject actor) {
        if(actor.CompareTag("Player")) {
            Debug.Log("Baobaogo got hit/fell!");
            Time.timeScale = 0;
        }
    }
    void OnDestroy() {
        ActorEvents.current.onCliffEnter -= ActorDies;
        ActorEvents.current.onMeroTouch -= ActorDies;
    }
}
