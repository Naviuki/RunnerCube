using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{

    public float cooldown;

    [HideInInspector] public bool isCooldown;

    private Image starImage;
    private PlayerController player;
    private Score _scoreText;

    void Start()
    {
        starImage = GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        isCooldown = true;

    }

    void Update()
    {
        if(isCooldown)
        {
            starImage.fillAmount -= 1 / cooldown * Time.deltaTime;
            if(starImage.fillAmount <= 0)
            {
                starImage.fillAmount = 1;
                isCooldown = false;
                player.star.SetActive(false);
                gameObject.SetActive(false);
                player.score.scoreMultiplier = 1;
            }
        }
    }

    public void ResetTimer()
    {
        starImage.fillAmount = 1;
    }







}

