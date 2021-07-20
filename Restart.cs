using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class Restart : MonoBehaviourPun
{
    public GameObject restartButton;
    public Text text;
    private void Start()
    {
        ScoreBank scoreBank = FindObjectOfType<ScoreBank>();
        text.text = "Game over, Score: "+scoreBank.score;
        if (!PhotonNetwork.IsMasterClient)
        {
            restartButton.SetActive(false);
            text.text += " waiting for restart";
        }
        
    }
    // Start is called before the first frame update
    public void RestartClicked()
    {   

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LoadLevel("GameScene"); 
    }
    
}
