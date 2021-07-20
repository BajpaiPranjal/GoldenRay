using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SpawnManager : MonoBehaviour
{
    public float minX, maxX, minY, maxY;
    public GameObject masterPlayer, guestPlayer;

    //public GameObject ray;
    // Start is called before the first frame update
    void Awake()
    {
       // Instantiate(ray, Vector3.zero, Quaternion.identity);
        if ( guestPlayer == null || masterPlayer == null) { Debug.LogError("PLAYER PREFAB MISSING!"); }
        #region Spawning Player
        //Vector2 randomPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        if (PhotonNetwork.IsMasterClient) 
        {
            PhotonNetwork.Instantiate(masterPlayer.name, new Vector2(-2,0), Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(guestPlayer.name, new Vector2(2, 0), Quaternion.identity);
        }
        #endregion
        
    }

    
}
