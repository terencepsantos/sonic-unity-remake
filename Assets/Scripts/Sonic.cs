using UnityEngine;
using System.Collections;
//using System;

public class Sonic : MonoBehaviour, ITakeDamage
{
    #region Private Fields
    private Animator playerAnimator;
    private Rigidbody2D playerRigidbody;
    private CircleCollider2D playerCollider;

    private Coroutine audioBreakOut;

    private float acceleration;
    private float velocityX;
    private float absoluteMagnitude;
    private bool isGrounded;
    private bool isInvincible;

    [SerializeField]
    private float
        playerNormalDrag = 5,
        playerChargedRunningDrag = 3,
        groundAcceleration = 70,
        airAcceleration = 100,
        jumpForce = 25,
        runVelocityThreshold = 8,
        chargedRunVelocityThreshold = 12,
        chargedImpulse = 2,
        timeInvincibleAfterDamage = 3.5f;

    [SerializeField]
    private int
        maxRingsAmountFromDamage = 10,
        scorePerRing = 50;

    private int ringsAmount;
    private int livesAmount;
    private int scoreAmount;

    #endregion

    #region Public Fields and Props
    //public LevelUIManager LevelUIManagerObj;
    public SpriteRenderer PlayerSpriteRenderer;
    public GameObject RingPrefab;

    public enum AnimState
    {
        IsIdle,
        IsWalking,
        IsRunning,
        IsJumping,
        IsDucking,
        IsCharging,
        IsChargedRunning,
        IsBreaking,
        IsTakingDamage,
        IsDying
    }
    public AnimState animState { get; private set; }

    public int Health { get; set; }

    #endregion


    void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CircleCollider2D>();

        gameObject.tag = "Player";
    }

    void Start()
    {
        animState = AnimState.IsIdle;
        SetInitialHealth(1);
        isGrounded = true;
        isInvincible = false;
        livesAmount = 1;
        ringsAmount = 0;
        UpdateHUDInfo();

        AudioManager.Instance.PlayLoop(AudioManager.AudioClipsEnum.Level1BG);
    }


    void Update()
    {
        //Horizontal Input
        velocityX = Input.GetAxis("Horizontal");
        absoluteMagnitude = Mathf.Abs(playerRigidbody.velocity.magnitude);

        if (isGrounded)
        {
            acceleration = groundAcceleration;

            //Taking Damage State
            if (animState == AnimState.IsTakingDamage)
            {
                velocityX = 0;
            }

            //Duck State
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                velocityX = 0;

                if (animState == AnimState.IsIdle || animState == AnimState.IsWalking || animState == AnimState.IsDucking || animState == AnimState.IsJumping)
                {
                    animState = AnimState.IsDucking;
                    playerRigidbody.velocity = new Vector2(0, 0);
                }

                //Charge (to run) State
                if (Input.GetButtonDown("Fire1") && (animState == AnimState.IsDucking || animState == AnimState.IsCharging))
                {
                    animState = AnimState.IsCharging;
                    AudioManager.Instance.PlayOneShot(AudioManager.AudioClipsEnum.BeginCharge);
                }
            }
            else
            {
                if (absoluteMagnitude == 0)
                {
                    //Idle State
                    if (animState == AnimState.IsWalking || animState == AnimState.IsJumping ||
                        animState == AnimState.IsDucking || animState == AnimState.IsChargedRunning ||
                        animState == AnimState.IsTakingDamage)
                    {
                        animState = AnimState.IsIdle;
                        playerRigidbody.drag = playerNormalDrag;
                    }

                    //Charged Run State
                    if (animState == AnimState.IsCharging)
                    {
                        animState = AnimState.IsChargedRunning;
                        ChargedRun();
                        playerRigidbody.drag = playerChargedRunningDrag;
                        AudioManager.Instance.PlayOneShot(AudioManager.AudioClipsEnum.ReleaseCharge);
                    }
                }
                else if (absoluteMagnitude > 0 && absoluteMagnitude < runVelocityThreshold)
                {
                    //Walk State
                    animState = AnimState.IsWalking;
                    playerRigidbody.drag = playerNormalDrag;
                    CheckAndTurnPlayerSprite();
                }
                else if (absoluteMagnitude > runVelocityThreshold && absoluteMagnitude < chargedRunVelocityThreshold)
                {
                    //Run State
                    animState = AnimState.IsRunning;
                    playerRigidbody.drag = playerNormalDrag;
                    CheckAndTurnPlayerSprite();
                }
            }

            //Jump State
            if (Input.GetButtonDown("Jump"))
            {
                if (absoluteMagnitude < chargedRunVelocityThreshold)
                    animState = AnimState.IsJumping;

                Jump();
                isGrounded = false;
                AudioManager.Instance.PlayOneShot(AudioManager.AudioClipsEnum.Jump);
            }
        }
        else
        {
            acceleration = airAcceleration;
        }

        UpdateAnimConditions();
    }


    void FixedUpdate()
    {
        //playerRigidbody.AddForce(Vector2.right * velocityX * acceleration);
        playerRigidbody.AddForce(transform.right * velocityX * acceleration);

        //if (playerRigidbody.velocity.x != 0)
        //    Debug.Log("velocity: " + playerRigidbody.velocity.x);

        //Breaking - State
        float normvelx = Mathf.Clamp(playerRigidbody.velocity.x, -1, 1);
        float absdiffx = Mathf.Abs(normvelx - velocityX);
        //playerAnimator.SetFloat("Break", absdiffx);

        if (absdiffx > 1 && audioBreakOut == null)
            audioBreakOut = StartCoroutine(OneShotBreak());

        //Check Ground
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, -transform.up);

        if (hit.collider != null) //Grounded
        {
            //transform.up = hit.normal;
            transform.up = Vector3.Lerp(transform.up, hit.normal, Time.fixedDeltaTime * 10);
        }
        else //In the air
        {
            //transform.up = Vector3.up;
            transform.up = Vector3.Lerp(transform.up, Vector3.up, Time.fixedDeltaTime * 10);
        }
    }


    private void UpdateAnimConditions()
    {
        //Updating Animator Conditions
        playerAnimator.SetBool("IsIdle", animState == AnimState.IsIdle);
        playerAnimator.SetBool("IsWalking", animState == AnimState.IsWalking);
        playerAnimator.SetBool("IsRunning", animState == AnimState.IsRunning);
        playerAnimator.SetBool("IsJumping", animState == AnimState.IsJumping);
        playerAnimator.SetBool("IsDucking", animState == AnimState.IsDucking);
        playerAnimator.SetBool("IsCharging", animState == AnimState.IsCharging);
        playerAnimator.SetBool("IsChargedRunning", animState == AnimState.IsChargedRunning);
        playerAnimator.SetBool("IsBreaking", animState == AnimState.IsBreaking);
        playerAnimator.SetBool("IsTakingDamage", animState == AnimState.IsTakingDamage);
        playerAnimator.SetBool("IsDying", animState == AnimState.IsDying);
    }


    private IEnumerator OneShotBreak()
    {
        AudioManager.Instance.PlayOneShot(AudioManager.AudioClipsEnum.Break);
        yield return new WaitForSeconds(1);
        StopCoroutine(audioBreakOut);
        audioBreakOut = null;
    }


    private void CheckAndTurnPlayerSprite()
    {
        if (velocityX > 0f) //Flip player sprite - Look right
            gameObject.transform.localScale = new Vector3(1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);

        if (velocityX < 0f) //Flip player sprite - Look left
            gameObject.transform.localScale = new Vector3(-1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }


    private void Jump()
    {
        playerRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }


    private void JumpBackFromDamage()
    {
        playerRigidbody.AddForce(Vector2.up * (jumpForce / 2), ForceMode2D.Impulse);

        var newVector = gameObject.transform.localScale.x >= 0 ? Vector2.left : Vector2.right;
        playerRigidbody.AddForce(newVector * (jumpForce / 2), ForceMode2D.Impulse);
    }


    private void ChargedRun()
    {
        if (gameObject.transform.localScale.x == 1)
            playerRigidbody.AddForce(Vector2.right * acceleration * chargedImpulse, ForceMode2D.Impulse);

        if (gameObject.transform.localScale.x == -1)
            playerRigidbody.AddForce(-Vector2.right * acceleration * chargedImpulse, ForceMode2D.Impulse);
    }


    private void CollectedRing(bool isRingRespawn)
    {
        if (!isRingRespawn)
            scoreAmount += scorePerRing;

        ringsAmount++;
        AudioManager.Instance.PlayOneShot(AudioManager.AudioClipsEnum.RingCollect);

        if (animState != AnimState.IsDying && Health <= 1)
            Health++;

        if (ringsAmount >= 100)
        {
            livesAmount++;
            ringsAmount = 0;
            AudioManager.Instance.PlayOneShot(AudioManager.AudioClipsEnum.LifeUp);
        }

        UpdateHUDInfo();
    }


    public void TakeDamage()
    {
        if (isInvincible)
            return;

        isInvincible = true;
        Health--;
        JumpBackFromDamage();
        animState = AnimState.IsTakingDamage;

        if (Health <= 0)
        {
            Death();
            return;
        }

        ScatterRings();
        Invoke("ResetInvincibleState", timeInvincibleAfterDamage);
        StartCoroutine(PlayerBlink());
    }


    private IEnumerator PlayerBlink()
    {
        while (isInvincible)
        {
            PlayerSpriteRenderer.enabled = !PlayerSpriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
        }

        PlayerSpriteRenderer.enabled = true;
    }


    private void ScatterRings()
    {
        int ringsAmountFromDamage = ringsAmount <= maxRingsAmountFromDamage ? ringsAmount : maxRingsAmountFromDamage;

        
        for (int i = 0; i < ringsAmountFromDamage; i++)
        {
            float[] randomForces = new float[3];

            randomForces[0] = Random.Range(jumpForce / 3, jumpForce / 2); //Force for X
            randomForces[1] = Random.Range(jumpForce / 3, jumpForce / 2); //Force for Y
            randomForces[2] = gameObject.transform.localScale.x >= 0 ? -1 : 1; //Direction to throw rings

            var obj = Instantiate(RingPrefab, gameObject.transform.localPosition, Quaternion.identity) as GameObject;
            obj.SendMessage("ScatteredRings", randomForces);
        }

        ringsAmount = 0;
        UpdateHUDInfo();
    }


    public void Death()
    {
        animState = AnimState.IsDying;
        playerCollider.enabled = false;

        livesAmount--;
        UpdateHUDInfo();
    }


    private void ResetInvincibleState()
    {
        isInvincible = false;
    }


    public void SetInitialHealth(int initialHealth)
    {
        Health = initialHealth;
    }


    private void UpdateHUDInfo()
    {
        LevelUIManager.Instance.RingsAmount.text = ringsAmount.ToString();
        LevelUIManager.Instance.LivesAmount.text = livesAmount.ToString();
        LevelUIManager.Instance.ScoreAmount.text = scoreAmount.ToString();
    }


    #region Collisions

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 8)
        {
            isGrounded = true;
        }

        if (coll.gameObject.CompareTag("Enemy"))
        {
            if (animState == AnimState.IsIdle || animState == AnimState.IsWalking ||
                animState == AnimState.IsRunning || animState == AnimState.IsDucking ||
                animState == AnimState.IsBreaking)
            {
                TakeDamage();
            }
            else
            {
                coll.collider.enabled = false;
                coll.gameObject.SendMessage("TakeDamage");
            }
        }
    }


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Ring"))
            CollectedRing(false);

        if (coll.CompareTag("RingRespawn"))
            CollectedRing(true);

        if (coll.CompareTag("EnemyBullet"))
            TakeDamage();
    }

    #endregion
}
