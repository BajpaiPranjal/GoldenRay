using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PlayerController : MonoBehaviour
{
    //Scrit refrences
    Health healthScript;
    LineRenderer lineRenderer;
    RayBehaviour rayBehaviour;
    //public Joystick joystick;

    //Components
    PhotonView pv;
    AudioSource _audioSource;
    private GameObject Blade;

    //Variables
    public float speed;
    public float dashSpeed, dashTime;
    public float resetSpeed;
    public int maxDist = 6;
    bool pickup = true;
    bool pvIsMine;

    Joystick joystick;

    // Start is called before the first frame update
    void Start()
    {
        resetSpeed = speed;
        pv = GetComponent<PhotonView>(); pvIsMine = pv.IsMine;
        healthScript = FindObjectOfType<Health>();
        lineRenderer = FindObjectOfType<LineRenderer>();
        lineRenderer.forceRenderingOff = true;
        _audioSource = GetComponent<AudioSource>();

        rayBehaviour = FindObjectOfType<RayBehaviour>();
        rayBehaviour.IncChainLength += MaxDistPlus2;

        Blade = GetComponentInChildren<BladeSP>(true).gameObject;
        Blade.SetActive(false);

        SpriteRenderer spBlade = Blade.GetComponent<SpriteRenderer>(); spBlade.sortingOrder = -10;
        if (PhotonNetwork.IsMasterClient) { pickup = false; }

        if (pv.IsMine)
        {
            joystick = FindObjectOfType<Joystick>();
            joystick.DeadZone = 0.3f;
        }
        //joystick.DeadZone = 0.5f;
        //if (!pv.IsMine) { joystick.enabled = false; }
        
    }

    private void Update()
    {   if (PhotonNetwork.CurrentRoom.PlayerCount == 2) lineRenderer.forceRenderingOff = false;

        if (pv.IsMine)
        {
            Vector2 ip = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            //Vector2 ip = joystick.Direction;
            Vector2 move = ip.normalized * speed * Time.deltaTime;
            
            Vector2 newpost = move + new Vector2(transform.position.x, transform.position.y);

            //measuring dist b/w remote player's end and new post

            if (Vector2.Distance(newpost, lineRenderer.GetPosition(1)) <= maxDist)
            {
                transform.Translate(move); 
            }
            
            if (!lineRenderer.forceRenderingOff)
            { lineRenderer.SetPosition(0, transform.position); }
            //0 index of line render is at local player's end and 1 index at remote playe's end
            if (Input.GetKeyDown(KeyCode.Space) && ip!= Vector2.zero)
            {
                StartCoroutine(Dash()); 
            }
        }
        else
        {
            if (!lineRenderer.forceRenderingOff)
            { lineRenderer.SetPosition(1, transform.position); }      
        }
    }

    private void MaxDistPlus2()
    {
        maxDist += 2;// Debug.Log("Action recieved: " + maxDist);
    }

    IEnumerator Dash()
    {
        speed = dashSpeed;
        yield return new WaitForSeconds(dashTime); 
        speed = resetSpeed;
    }

    #region Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") )
        {
            _audioSource.Play(); // Plays audio Hero_hit
            if (pv.IsMine)
            {
                 healthScript.TakeDamage(); // health is synced via RPC
            }
            
        }

        if (collision.CompareTag("Ruby") )
        {

            //PhotonNetwork.Destroy(collision.gameObject);

            CallRpc();
            DestroyGO(collision.gameObject);
            #region destryGO
            //Destroy(collision.gameObject);     // destroying game object locally on all clients - no lag


            //// if object is still active on any client then destroying in sync 
            //if (collision.gameObject.activeSelf && PhotonNetwork.IsMasterClient)    //
            //{

            //    int viewID = collision.gameObject.GetPhotonView().ViewID;
            //    if (viewID != 0)
            //        Destroy(PhotonView.Find(viewID).gameObject);
            //    //PhotonNetwork.Destroy(this.gameObject);  

            //}
            #endregion
        }
    }
    #endregion

    public void DestroyGO( GameObject target)
    {
        
        Destroy(target);
        if (target.activeSelf && PhotonNetwork.IsMasterClient)    //
        {

            int viewID = target.GetPhotonView().ViewID;
            if (viewID != 0)
                Destroy(PhotonView.Find(viewID).gameObject);
            //PhotonNetwork.Destroy(this.gameObject);  

        }
    }

    public void CallRpc()
    {
       if(pv.IsMine) pv.RPC("powerUp", RpcTarget.All,pickup); //pv.isMine ensures only on client calls RPC
    }

    [PunRPC]

    private void powerUp(bool token)
    {
        if (token)
        {
            StartCoroutine(EnableBlades());
        }
        else
        {
            //RayBehaviour rayBehaviour = FindObjectOfType<RayBehaviour>();
            rayBehaviour.MakeRayElectric();
        }
        pickup = !token;
    }

    IEnumerator EnableBlades()
    {
        Blade.SetActive(true);

        yield return new WaitForSeconds(6);

        Blade.SetActive(false);
    }

}
