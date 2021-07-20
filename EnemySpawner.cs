using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class EnemySpawner : MonoBehaviour
{
    //Script refreces
    public GameObject enemyBot;
    public Transform[] spawnPoints;
    public GameObject feedbackPanel;
    
    
    // Variables
    float timeBWspawn, nxtSpawnTime;
    int count = 0, wave_num = 1;
    private bool canSpawn;
    private int minPlayers = 2, maxEnemies = 5;
    [HideInInspector]
    public float enemySpeed;
    
    public enum SpawnState { waveChange, spwaning};

    public SpawnState state;// = SpawnState.counting;

    public const byte myEventCode = 2;
    bool master;
    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    // Start is called before the first frame update
    void Start()
    {
        nxtSpawnTime = 5;
        timeBWspawn = 5f;
        canSpawn = true;
        enemySpeed = 1.5f;
        state = SpawnState.spwaning;
        //if MC
        master = PhotonNetwork.IsMasterClient;
        if(master) StartCoroutine(DoCheck());
        
    }

    

    IEnumerator DoCheck()
    {
        for (; ; )
        {
            Spawn();
            yield return new WaitForSeconds(timeBWspawn);
        }
    }

    void Spawn()
    {
        if (count < maxEnemies && canSpawn && state == SpawnState.spwaning ) //0 > nxtSpawnTime &&  PhotonNetwork.CurrentRoom.PlayerCount == minPlayers &&
        {
            //Instantiate(enemyBot, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(enemyBot.name, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
            }
            count += 1;//nxtSpawnTime += timeBWspawn; 
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //if !MC return;
        if (!master) return;
        // EnemySpawner will run on master client to keep things in sync



         if (count == maxEnemies && FindObjectOfType<Enemy>() == null)
         {
           
            count = 0; canSpawn = false;

            StopCoroutine(DoCheck());

            //Raise event NextWave
            
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            //SendOptions sendOptions = SendOptions.SendReliable;
            PhotonNetwork.RaiseEvent(myEventCode,null,raiseEventOptions,SendOptions.SendReliable);

        }

    }

    // Recieving Event On all clients (Receivers = ReceiverGroup.All)
    private void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code == 2)
        {
            StartCoroutine(NextWave());
        }
    }


    // only this function runs on both client 
    IEnumerator NextWave()
    {
        state = SpawnState.waveChange;

        yield return new WaitForSeconds(4);
        feedbackPanel.SetActive(true);
        feedbackPanel.GetComponentInChildren<Text>().text = "W A V E  C L E A R E D!";
        //Debug.Log("W A V E  C L E A R E D!");

        yield return new WaitForSeconds(4);

        wave_num += 1; enemySpeed += 0.25f; maxEnemies += 1;

       // Debug.Log("N E X T  W A V E: " + wave_num);
        feedbackPanel.GetComponentInChildren<Text>().text = "N E X T  W A V E: " + wave_num.ToString();

        yield return new WaitForSeconds(4);
        feedbackPanel.SetActive(false);

        //Debug.Log("LAUNCHED");
        canSpawn = true;
        state = SpawnState.spwaning;
        //if MC
        if (PhotonNetwork.IsMasterClient)  StartCoroutine(DoCheck());
    }

    public void StopWaves()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StopCoroutine(DoCheck()); Debug.Log("STOP WAVES CALLED!");
        }
        
    }

    public void ResetWaves()
    {
        wave_num = 1; enemySpeed = 1.5f;
        maxEnemies = 5; count = 0;
        state = SpawnState.spwaning;
        canSpawn = true;

        if (PhotonNetwork.IsMasterClient) StartCoroutine(DoCheck());
    }
}
