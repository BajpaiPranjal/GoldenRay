using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class Health : MonoBehaviourPun
{
    public Text healthDisplay;
    public GameObject gameOver, scoreCounter;
    public int healthValue = 20;
    PhotonView pV;
    private void Start(){
       healthDisplay.text = "start: "+healthValue.ToString();
       pV = GetComponent<PhotonView>();
          
    }

    public void TakeDamage(){
        pV.RPC("TakeDamageRPC", RpcTarget.All);
    } 
    
    [PunRPC]

    private void TakeDamageRPC()
    {
        healthValue -= 10;
        if (healthValue <= 0) {
            // Stop waves
            
            //EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
            //if (enemySpawner.isActiveAndEnabled)
            //{ enemySpawner.enabled = false; gameOver.SetActive(true); }
            //gameOver.SetActive(true);
            //scoreCounter.SetActive(true);
            ScoreBank scoreBank = FindObjectOfType<ScoreBank>();

            scoreBank.score = FindObjectOfType<ScoreManager>().kills;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel("GameOver");
        }    
        healthDisplay.text = "Health: " + healthValue.ToString();
    }
    
}
