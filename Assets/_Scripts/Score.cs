using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] public Text scoreText;
    private int totalScore;
    private Star starBonus;

    public int scoreMultiplier;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Star")
        {
            if(starBonus.isCooldown)
            {
                scoreText.color = Color.red;
                Debug.Log("Star");
            }

        }
        else
        {
            scoreText.color = Color.black;
        }     
    }

    private void FixedUpdate()
    {
        totalScore += scoreMultiplier;
        scoreText.text = totalScore.ToString();
  
    }
}