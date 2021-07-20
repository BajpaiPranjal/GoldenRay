using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Enemy : MonoBehaviour
{
    //Script refrences
    PlayerController[] players;
    PlayerController nearestPlayer;
    ScoreManager scoreManager;
    EnemySpawner enemySpawner;

    //Component refrences
    PhotonView pv;

    //variables
    private float enemySpeed;
    private bool isQuitting;

    public GameObject particlePrefab;
    public GameObject rubyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<PlayerController>();  nearestPlayer = players[0];

        scoreManager = FindObjectOfType<ScoreManager>();

        enemySpawner = FindObjectOfType<EnemySpawner>();

        enemySpeed = enemySpawner.enemySpeed;
        //_audioSource = GetComponent<AudioSource>();

        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(" players[] length is: " +players.Length);
        float distOne, distTwo;
        distOne = Vector2.Distance(players[0].transform.position, transform.position);
        if (players.Length > 1)
        {
            distTwo = Vector2.Distance(players[1].transform.position, transform.position);
            if (distOne < distTwo)
            {
                nearestPlayer = players[0];
            }
            else
            {
                nearestPlayer = players[1];
            }
        }

        if (nearestPlayer != null)
        {
            transform.Translate((nearestPlayer.transform.position - transform.position).normalized * enemySpeed * Time.deltaTime);
        }
        else { Debug.LogError("nearest player is null"); /*Debug.Log(nearestPlayer);*/ }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.CompareTag("Player")) )
        {
            //Sound is played via playerController
            // Score is synced via RPC call on master client
            pv.RpcSecure("DestroyThisEnemy", RpcTarget.All, true);
            #region OldDestroy Method
            //Destroy(this.gameObject);     // destroying game object locally on all clients - no lag


            //// if object is still active on any client then destroying in sync 
            //if (this.gameObject.activeSelf && PhotonNetwork.IsMasterClient)    //
            //{

            //    int viewID = this.gameObject.GetPhotonView().ViewID;
            //    if ( viewID!= 0)
            //        Destroy(PhotonView.Find(viewID).gameObject);
            //    //PhotonNetwork.Destroy(this.gameObject);  

            //}
            // Enemy is destroyed in sync 
            #endregion
        }


        if (collision.CompareTag("Ray"))
        {
            
             
            if (PhotonNetwork.IsMasterClient)
            {
                scoreManager.AddScore();
            }
            if (PhotonNetwork.IsMasterClient && (Random.value < 1))
            {

                PhotonNetwork.Instantiate(rubyPrefab.name, transform.position, Quaternion.identity);
            }

            pv.RpcSecure("DestroyThisEnemy", RpcTarget.All,true);

            #region OldDestroy Method
            //Destroy(this.gameObject);     // destroying game object locally on all clients - no lag


            //// if object is still active on any client then destroying in sync 
            //if (this.gameObject.activeSelf && PhotonNetwork.IsMasterClient)    //
            //{

            //    int viewID = this.gameObject.GetPhotonView().ViewID;
            //    if (viewID != 0)
            //        Destroy(PhotonView.Find(viewID).gameObject);
            //    //PhotonNetwork.Destroy(this.gameObject);  

            //}
            #endregion
            
        }
    }

    private void OnDestroy()
    {
        //Debug.Log("On Destroy called"); 
        if (!isQuitting)
        {
            // locally instantiate death effect
            Instantiate(particlePrefab, this.gameObject.transform.position, Quaternion.identity);
            // play sound on enemy's death 
            
            
        }

    }
    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    [PunRPC]

    void DestroyThisEnemy()
    {
        Destroy(this.gameObject);
    }
}
