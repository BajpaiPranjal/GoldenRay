using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MainMenu : MonoBehaviourPunCallbacks
{
    public InputField createInput, joinInput;
    public Text loadingStatus, feedbackText;
    public GameObject feedbackPanel, retryButton, quitButton;
    // Start is called before the first frame 

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2; roomOptions.CleanupCacheOnLeave = true;
        roomOptions.EmptyRoomTtl = 1;
        PhotonNetwork.CreateRoom(createInput.text,roomOptions);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        
        loadingStatus.text = PhotonNetwork.LevelLoadingProgress.ToString();
        PhotonNetwork.LoadLevel("GameScene");          
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //base.OnCreateRoomFailed(returnCode, message);
        feedbackPanel.SetActive(true);
        feedbackText.text = message;        
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //base.OnJoinRoomFailed(returnCode, message);
        feedbackPanel.SetActive(true); feedbackText.text = message;
    }

    public void RetryClicked()
    {
        feedbackPanel.SetActive(false);
    }

    public void QuitClicked()
    {
        Application.Quit();
    }
}
