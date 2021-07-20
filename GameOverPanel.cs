using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class GameOverPanel : MonoBehaviour
{
    public Text gameOverText;
    public GameObject waitingText, restartButton;

    
    PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        gameOverText.text = "GAME OVER! Kills: " + FindObjectOfType<ScoreManager>().score.text; //+"\n"
        if (PhotonNetwork.IsMasterClient)
        {
            waitingText.SetActive(false);
            restartButton.SetActive(true);
        }
        else
        {
            waitingText.SetActive(true);
            restartButton.SetActive(false); 
        }

    }

    public void RestartClicked() //This function will called on MasterClient only
    {
        //photonView.RPC("Restart", RpcTarget.All); 
        // reset wave to 1
        

        //remove gameOver Screen
       

        // score to 0

        // Health to full
    }
    //[PunRPC]

    void Restart()
    {
        //PhotonNetwork.LoadLevel("GameScene");
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner.ResetWaves();
        

        this.gameObject.SetActive(false);
    }
}
