using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class ScoreManager : MonoBehaviour
{
   
    public Text score;
    PhotonView photonView;
    [HideInInspector]
    public int kills = 0;
    
    // 
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        score.text = "Kills: "+kills.ToString();
    }
    public void AddScore()
    {
        ///Calling RPC Function here;
        photonView.RPC("AddScoreRPC", RpcTarget.All);
    }
    [PunRPC]

    private void AddScoreRPC()
    {
        kills += 1;
        score.text = "Kills: " + kills.ToString();
    }


    
}