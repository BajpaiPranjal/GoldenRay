using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Realtime;


public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public GameObject retryButton;
    public Text feedBackText;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); feedBackText.text = "CONNECTING...";
        
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void OnFailedToConnect()
    {
        retryButton.SetActive(true); feedBackText.text = "Connection Failed";
    }

    public void OnRetryClicked()
    {
        PhotonNetwork.ConnectUsingSettings(); retryButton.SetActive(false);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        retryButton.SetActive(true); feedBackText.text = "Connection Failed";
        //base.OnDisconnected(cause);
    }

}
