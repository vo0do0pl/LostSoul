using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCosmicManController : MonoBehaviour
{
	#region Properties
	public float playerSpeed;
	public float jumpingSpeed;
	public float hurtCounter;

	#region Ground check properties
	public Transform groundCheck;
	public float groundCheckRadius;
	public LayerMask groundLayer;
	#endregion

	#endregion

	#region Private
	private Rigidbody2D playerRigidbody;
	private Animator playerAnimator;
	private bool isPlayerOnGround;
	private float vHurtCounter;
	private float vShootingCounter;
	private bool facingRight;
	public int bulletsAmount = 10;
	private int bulletIndex;
	private WaitForSeconds wait;

	#endregion

	bool isPlayerStopped = false;
    private readonly int teleportID = Animator.StringToHash("Teleport");
    private readonly int playerSpeedID = Animator.StringToHash("PlayerSpeed");
    private readonly int hurtID = Animator.StringToHash("Hurt");
    private readonly int onGroundID = Animator.StringToHash("OnGround");

	[SerializeField] GameObject deadBody;

	public static event Action PlayerDied;
	public static event Action PlayerMoved;

	// Start is called before the first frame update
	void Start()
    {
        TutorialManager.StopPlayerFromMoving += OnStopPlayerFromMoving;
        TutorialManager.LetPlayerMove += OnLetPlayerMove;

        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        facingRight = true;
        wait = new WaitForSeconds(1.5f);
    }

    private void OnDestroy()
    {
        TutorialManager.StopPlayerFromMoving -= OnStopPlayerFromMoving;
        TutorialManager.LetPlayerMove -= OnLetPlayerMove;
    }

    // Update is called once per frame
    void Update()
    {
		isPlayerOnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

		if (!isPlayerStopped && vHurtCounter <= 0f)
		{
			#region Horizontal Movement
			if (Input.GetAxisRaw("Horizontal") > 0f)
			{
				PlayerMoved?.Invoke();
				// Moving Forward
				playerRigidbody.velocity = new Vector3(playerSpeed, playerRigidbody.velocity.y, 0f);

				// Flip Sprite on X axis
				if (!facingRight)
				{
					FlipPlayer();
				}
			}
			else if (Input.GetAxisRaw("Horizontal") < 0f)
			{
				PlayerMoved?.Invoke();
				// Moving Forward
				playerRigidbody.velocity = new Vector3(-playerSpeed, playerRigidbody.velocity.y, 0f);

				// Flip Sprite on X axis
				if (facingRight)
				{
					FlipPlayer();
				}
			}
			else
			{
				playerRigidbody.velocity = new Vector3(0f, playerRigidbody.velocity.y, 0f);
			}
			#endregion

			#region Vertical Movement (Jump)
			if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Space)) && isPlayerOnGround)
			{
				PlayerMoved?.Invoke();
				playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, jumpingSpeed, 0f);
			}
			#endregion
		}
		else
		{
			vHurtCounter -= Time.deltaTime;
		}

		AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

		// Always checking if player on Ground or not
		playerAnimator.SetBool(onGroundID, isPlayerOnGround);

		// Always setting the Player Speed to the Animator - Idle if Horizontal PlayerSpeed < 0.05f
		playerAnimator.SetFloat(playerSpeedID, Mathf.Abs(playerRigidbody.velocity.x));
	}

	private void FlipPlayer()
	{
		facingRight = !facingRight; // FacingRight becomes the opposite of the current value.
		transform.Rotate(0f, 180f, 0f);
	}

	private void OnLetPlayerMove()
    {
        isPlayerStopped = false;
    }

    private void OnStopPlayerFromMoving()
    {
        isPlayerStopped = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            playerAnimator.SetTrigger(hurtID);
            vHurtCounter = hurtCounter;

			Instantiate(deadBody, gameObject.transform.position, Quaternion.identity);
			PlayerDied?.Invoke();
			Destroy(gameObject);
		}
    }
}
