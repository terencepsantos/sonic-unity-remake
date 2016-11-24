using UnityEngine;
using System.Collections;

public class Sonic : MonoBehaviour
{
    #region Private Fields
    private Animator playerAnimator;
    private Rigidbody2D playerRigidbody;

    private float acceleration;
    private float velocityX;
    private float absoluteMagnitude;
    private bool isGrounded;

    [SerializeField]
    private float
        playerNormalDrag = 5,
        playerChargedRunningDrag = 3,
        groundAcceleration = 70,
        airAcceleration = 100,
        jumpForce = 25,
        runVelocityThreshold = 8,
        chargedRunVelocityThreshold = 12,
        chargedImpulse = 2;

    #endregion

    #region Public Fields and Props
    public enum State
    {
        IsIdle,
        IsWalking,
        IsRunning,
        IsJumping,
        IsDucking,
        IsCharging,
        IsChargedRunning,
        IsDying
    }
    public State state { get; private set; }


    #endregion


    void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        state = State.IsIdle;
        isGrounded = true;

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

            //Duck State
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                velocityX = 0;

                if (state == State.IsIdle || state == State.IsWalking || state == State.IsDucking || state == State.IsJumping)
                {
                    state = State.IsDucking;
                    playerRigidbody.velocity = new Vector2(0, 0);
                }

                //Charge (to run) State
                if (Input.GetButtonDown("Fire1") && (state == State.IsDucking || state == State.IsCharging))
                {
                    state = State.IsCharging;
                    AudioManager.Instance.PlayOneShot(AudioManager.AudioClipsEnum.BeginCharge);
                }
            }
            else
            {
                if (absoluteMagnitude == 0)
                {
                    //Idle State
                    if (state == State.IsWalking || state == State.IsJumping || state == State.IsDucking || state == State.IsChargedRunning)
                    {
                        state = State.IsIdle;
                        playerRigidbody.drag = playerNormalDrag;
                    }

                    //Charged Running State
                    if (state == State.IsCharging)
                    {
                        state = State.IsChargedRunning;
                        ChargedRun();
                        playerRigidbody.drag = playerChargedRunningDrag;
                        AudioManager.Instance.PlayOneShot(AudioManager.AudioClipsEnum.ReleaseCharge);
                    }
                }
                else if (absoluteMagnitude > 0 && absoluteMagnitude < runVelocityThreshold)
                {
                    //Walk State
                    state = State.IsWalking;
                    playerRigidbody.drag = playerNormalDrag;
                    CheckAndTurnPlayerSprite();
                }
                else if (absoluteMagnitude > runVelocityThreshold && absoluteMagnitude < chargedRunVelocityThreshold)
                {
                    //Run State
                    state = State.IsRunning;
                    playerRigidbody.drag = playerNormalDrag;
                    CheckAndTurnPlayerSprite();
                }
            }

            //Jump State
            if (Input.GetButtonDown("Jump"))
            {
                state = State.IsJumping;
                Jump();
                isGrounded = false;
                AudioManager.Instance.PlayOneShot(AudioManager.AudioClipsEnum.Jump);
            }
        }
        else
        {
            acceleration = airAcceleration;
        }


        //Updating Animator Conditions
        playerAnimator.SetBool("IsIdle", state == State.IsIdle);
        playerAnimator.SetBool("IsWalking", state == State.IsWalking);
        playerAnimator.SetBool("IsRunning", state == State.IsRunning);
        playerAnimator.SetBool("IsJumping", state == State.IsJumping);
        playerAnimator.SetBool("IsDucking", state == State.IsDucking);
        playerAnimator.SetBool("IsCharging", state == State.IsCharging);
        playerAnimator.SetBool("IsChargedRunning", state == State.IsChargedRunning);
        playerAnimator.SetBool("IsDying", state == State.IsDying);
    }


    void FixedUpdate()
    {
        //if (state != State.IsChargedRunning)
        //    playerRigidbody.AddForce(Vector2.right * velocityX * acceleration);
        //else
        //    playerRigidbody.AddForce(Vector2.right * acceleration);

        playerRigidbody.AddForce(Vector2.right * velocityX * acceleration);

        if (playerRigidbody.velocity.x != 0)
        Debug.Log("velocity: " + playerRigidbody.velocity.x);
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


    private void ChargedRun()
    {
        if (gameObject.transform.localScale.x == 1)
            playerRigidbody.AddForce(Vector2.right * acceleration * chargedImpulse, ForceMode2D.Impulse);

        if (gameObject.transform.localScale.x == -1)
            playerRigidbody.AddForce(-Vector2.right * acceleration * chargedImpulse, ForceMode2D.Impulse);
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 8)
        {
            isGrounded = true;
        }
    }
}
