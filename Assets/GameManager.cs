using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    int totalScore = 0;
    [SerializeField] private TMP_Text scoreCounter;
    [SerializeField] private GameObject player;
    private Player script;




    public static LayerMask enemyLayers;

    [SerializeField] private Transform AiPath;

    public static Transform Paths { get; private set; }

    


    private void Start()
    {
        script = player.GetComponent<Player>();

        enemyLayers = 1 << LayerMask.NameToLayer("EnemyRoot");
    }

    public void Ammo(int amount)
    {
        script.AddAmmo(amount);
    }
    public void AddScore(int pointValue)
    {
        
        totalScore += pointValue;
        Debug.Log(totalScore);
        scoreCounter.text = "Score: " + totalScore.ToString();
    }
}
