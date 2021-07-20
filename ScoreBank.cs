using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBank : MonoBehaviour
{
    public int score; //int health;
    private void Start()
    {
        
        //score = scoreManager.kills;
        DontDestroyOnLoad(this.gameObject);
    }
}
