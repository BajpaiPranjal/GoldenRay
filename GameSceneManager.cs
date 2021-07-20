using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.Android;

public class GameSceneManager : MonoBehaviourPunCallbacks
{

    public GameObject feedbackScreen, PlayerSpawner, Hud, enemySpawner;// Ray;
    private void Start()
    {
        AndroidDevice.SetSustainedPerformanceMode(true); 
        if (PhotonNetwork.IsMasterClient) // if this client has created the room
        { feedbackScreen.SetActive(true); PlayerSpawner.SetActive(false); Hud.SetActive(false); enemySpawner.SetActive(false); }
        else //if this client has joned a rooom
        {
            PlayerSpawner.SetActive(true); Hud.SetActive(true); feedbackScreen.SetActive(false); enemySpawner.SetActive(true);
            //Instantiate(Ray, Vector3.zero, Quaternion.identity);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //this part of code will run on master client i.e. which is hosting 
        //base.OnPlayerEnteredRoom(newPlayer)
        //when player joined room remove waiting screen and spawn prefabs
        //feedbackScreen.GetComponentInChildren<Text>().text = "Player joined"; 

        PlayerSpawner.SetActive(true); Hud.SetActive(true); feedbackScreen.SetActive(false); enemySpawner.SetActive(true);

       // Instantiate(Ray, Vector3.zero, Quaternion.identity);
        
        
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //base.OnPlayerLeftRoom(otherPlayer);
        feedbackScreen.SetActive(true);  //DestroyImmediate(Ray,true);
        feedbackScreen.GetComponentInChildren<Text>().text = "Player left";

        StartCoroutine(Waitfor2Seconds()); 
    }

    IEnumerator Waitfor2Seconds()
    {
        yield return new WaitForSeconds(2); PhotonNetwork.LeaveRoom();
    }

    public void Leave()
    {
        //DestroyImmediate(Ray);
        PhotonNetwork.LeaveRoom(); 
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Main Menu");   
    }

    
}
