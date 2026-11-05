using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public ControlType controlType;
    public enum ControlType {Mobile, PC};

    [Header("PlayerControl")]
    private CharacterController controller;
    public Score score;
    private Vector3 vector3Player;
    public Animator animator;
    [SerializeField] public float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    public bool isImmortal;
    public bool isHardcore;

    private bool isSliding;

    [SerializeField] private string[] jumpAnimationStates;

    private int lineToMove = 1;
    public float lineDistance = 4; 
    private bool isMaxSpeed = false;

    [Header("Elements")]
    [SerializeField] public static int coins;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject scoreText;
    [SerializeField] private Text coinsText;
    [SerializeField] private Score scoreScript;
    [SerializeField] private AudioClip coinSound;
     
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hitSound;
    
    [SerializeField] private AudioSource bonusClip;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Transform playerTransform;

    [Header("Shield")]
    public GameObject shield;
    public Shield shieldTimer;
    [SerializeField] private AudioClip shieldSound;

    [Header("Star")]
    public GameObject star;
    public Star starTimer;
    [SerializeField] private AudioClip starSound;

    [Header("Dumbbell")]
    [SerializeField] private AudioClip DumbbellSound;


    [Header("Effects")]
    public GameObject _CoinEffect;
    public GameObject _ShieldEffect;
    public GameObject _StarEffect;
    public GameObject _DumbbellEffect;
    public GameObject _ShieldDeathEffect;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetJumpForce(float newJumpForce)
    {
        jumpForce = newJumpForce;
    }


    public void SetImmortal(bool newImmortal)
    {
        isImmortal = newImmortal;
    }

    public void SetHardcore(bool newHardcore)
    {
        isHardcore = newHardcore;
        if(isHardcore)
        {
            RenderSettings.fogDensity = 0.03f;
            speed += 70f;
        }
    }

    void PlayJumpAnimation()
    {
        int randomIndex = Random.Range(0, jumpAnimationStates.Length);
        string chosenJumpAnimation = jumpAnimationStates[randomIndex];
        animator.SetBool("isRunning", false);
        animator.Play(chosenJumpAnimation);
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        score = scoreText.GetComponent<Score>();
        score.scoreMultiplier = 1;
        Time.timeScale = 1;
        coins = PlayerPrefs.GetInt("coins");
        coinsText.text = coins.ToString();
        StartCoroutine(SpeedIncrease());
        isImmortal = false;
        isHardcore = false;
    }

    private void Update()
    {

            if (SwipeController.swipeRight)
            {
                if (lineToMove < 2)
                lineToMove++;
            }

            if (SwipeController.swipeLeft)
            {
                if (lineToMove > 0)
                lineToMove--;
            }

            if (SwipeController.swipeUp)
            {
                if (controller.isGrounded)
                    Jump();
            }

            if (SwipeController.swipeDown)
            {
                StartCoroutine(Slide());
            }

            if(controller.isGrounded && !isSliding)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }

            Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
            if (lineToMove == 0)
                targetPosition += Vector3.left * lineDistance;
            else if (lineToMove == 2)
                targetPosition += Vector3.right * lineDistance;

            if (transform.position == targetPosition)
                return;
                Vector3 diff = targetPosition - transform.position;
                Vector3 movevector3Player = diff.normalized * 25 * Time.deltaTime;
            if (movevector3Player.sqrMagnitude < diff.sqrMagnitude)
                controller.Move(movevector3Player);
            else
            {
                controller.Move(diff);
            }

            if (speed >= 110)
            {
                isMaxSpeed = true;
            }    

    }

    private void Jump()
    {
        vector3Player.y = jumpForce;
        //animator.SetTrigger("isJumping");
        //animator.SetTrigger("isSecondJump");
        //animator.SetTrigger("isThirdJump");
        PlayJumpAnimation();
    }

    void FixedUpdate()
    {
        vector3Player.z = speed;
        vector3Player.y += gravity * Time.fixedDeltaTime;
        controller.Move(vector3Player * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "obstacle")
        {
            if (isImmortal)
            {
                animator.SetTrigger("isDying");
                Instantiate(_ShieldDeathEffect, transform.position, Quaternion.identity);
                bonusClip.PlayOneShot(hitSound);
                Destroy(hit.gameObject);
                shieldTimer.TimerOver();

            }    

            else
            {
                StartCoroutine(Death());
                bonusClip.PlayOneShot(deathSound);
                //losePanel.SetActive(true);
                int lastRunScore = int.Parse(scoreScript.scoreText.text.ToString());
                PlayerPrefs.SetInt("lastRunScore", lastRunScore);
                //Time.timeScale = 0;
                pauseButton.SetActive(false);
                pausePanel.SetActive(false);
            }
        }

        if (hit.gameObject.tag == "hole")
        {
            if (isImmortal)
            {
                Instantiate(_ShieldDeathEffect, transform.position, Quaternion.identity);
                bonusClip.PlayOneShot(hitSound);
                Destroy(hit.gameObject);
                shieldTimer.TimerOver();

            }    

            else
            {
                bonusClip.PlayOneShot(deathSound);
                StartCoroutine(Death());
                //losePanel.SetActive(true);
                int lastRunScore = int.Parse(scoreScript.scoreText.text.ToString());
                PlayerPrefs.SetInt("lastRunScore", lastRunScore);
                //Time.timeScale = 0;
                pauseButton.SetActive(false);
                pausePanel.SetActive(false);
            }
        }        
    }

    private void OnTriggerEnter(Collider other)
    {            
        
        if (other.gameObject.tag == "Coin")
        {
            Instantiate(_CoinEffect, transform.position, Quaternion.identity);
            bonusClip.PlayOneShot(coinSound);
            CoinText.Coin += 1;
            PlayerPrefs.SetInt("Coins", CoinText.Coin);
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Shield")
        {
            bonusClip.PlayOneShot(shieldSound);
            Instantiate(_ShieldEffect, transform.position, Quaternion.identity);
            if(!shield.activeInHierarchy)
            {
                shield.SetActive(true);
                shieldTimer.gameObject.SetActive(true);
                shieldTimer.isCooldown = true;
                isImmortal = true;
                Destroy(other.gameObject);
            }
            else
            {
                shieldTimer.ResetTimer();
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.tag == "Star")
        {
            bonusClip.PlayOneShot(starSound);
            Instantiate(_StarEffect, transform.position, Quaternion.identity);
            if(!star.activeInHierarchy)
            {
                star.SetActive(true);
                starTimer.gameObject.SetActive(true);
                starTimer.isCooldown = true;
                score.scoreMultiplier = 2;
                Destroy(other.gameObject);
            }
            else
            {
                starTimer.ResetTimer();
                Destroy(other.gameObject);
            }  
        }

        if (other.gameObject.tag == "Dumbbell")
        {
            animator.SetTrigger("isSwallow");
            bonusClip.PlayOneShot(DumbbellSound);
            Instantiate(_DumbbellEffect, transform.position, Quaternion.identity);
            speed -= 15;
            Destroy(other.gameObject);
        }

    }

    private IEnumerator Death()
    {
        speed *= 0;
        animator.SetBool("isRunning", false);
        animator.SetTrigger("isDying");
        gravity = -100f;
        yield return new WaitForSeconds(0.003f);
        losePanel.SetActive(true);
        Time.timeScale = 0;

    }

    private IEnumerator SpeedIncrease()
    {
        yield return new WaitForSeconds(1);
        if (!isMaxSpeed)
        {
            speed++;
            StartCoroutine(SpeedIncrease());
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        controller.radius = 0.1f;
        controller.height = 0.77f;
        controller.center = new Vector3(0, 0, 0);
        animator.SetTrigger("isSliding");

        yield return new WaitForSeconds(1);

        isSliding = false;
        controller.center = new Vector3(0, 0.22f, 0);
        controller.radius = 0.77f;
        controller.height = 0f;
    }

}