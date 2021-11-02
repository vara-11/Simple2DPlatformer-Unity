using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputHandler : MonoBehaviour
{
    public Rigidbody2D playerRigidbody;
    public Animator animator;

    private Vector2 movementInput;
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }

    private bool isFacingRight = true;
    private bool isRun = false;
    private bool isGrounded = false;
    private bool canJump = false;

    public float movementSpeed = 10f;
    public float jumpForce = 16.0f;
    public float groundCheckRadious;

    public LayerMask groundLayerMask;
    public LayerMask portalLayer;

    public Transform groundCheck;

    public Transform itemPos;
    public static int diamondCount;
    public GameObject diamondNotification;

    public GameObject portalNotification;

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        NormInputX = Mathf.RoundToInt(movementInput.x);
        NormInputY = Mathf.RoundToInt(movementInput.y);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ApplyJump();
        }
        if (context.performed)
        {

        }
        if (context.canceled)
        {

        }
        Debug.Log("Jump Input");
    }

    public void OnPlaceItem(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlaceDiamondToPort();
        }
    }

    public void OnRestartGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        CheckGrounded();
        CheckPlayerJump();
    }

    private void LateUpdate()
    {
        UpdateAnimations();
    }

    private void UpdateMovement()
    {
        CheckMovementDirection();
        ApplyMovement(); 
    }

    public void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadious, groundLayerMask);
    }

    public void CheckPlayerJump()
    {
        if(isGrounded && playerRigidbody.velocity.y <= 0)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
    }

    private void CheckMovementDirection()
    {
        if(isFacingRight && NormInputX < 0)
        {
            FlipPlayer();
        }else if(!isFacingRight && NormInputX > 0)
        {
            FlipPlayer();
        }
    }

    private void ApplyMovement()
    {
        playerRigidbody.velocity = new Vector2(movementSpeed * NormInputX, playerRigidbody.velocity.y);
    }

    private void FlipPlayer()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180.0f, 0.0f);
    }

    private void ApplyJump()
    {
        if (canJump)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
        }
    }

    private void UpdateAnimations()
    {
        isRun = (playerRigidbody.velocity.x != 0) ? true : false;
        animator.SetBool("isRun", isRun);

        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", playerRigidbody.velocity.y);
    }

    private void PlaceDiamondToPort()
    {
        Collider2D portal = Physics2D.OverlapBox(transform.position, Vector3.one * 2f, 0f,portalLayer);
        if (portal != null && diamondCount > 0)
        {
            diamondCount--;
            if (diamondCount <= 0)
            {
                foreach (Transform item in itemPos)
                {
                    Destroy(item.gameObject);
                }
            }
            transform.position = portal.transform.GetComponent<Portal>().toPortposition.position + Vector3.one;
        }
        else
        {
            diamondNotification.SetActive(true);
            Invoke("DisableNotification", 3f);
        }
    }

    private void DisableNotification()
    {
        diamondNotification.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (collision.transform.tag == "GameOver")
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Portal")
        {
            portalNotification.SetActive(true);
            Invoke("DisablePortalNotification", 2f);
        }
    }

    private void DisablePortalNotification()
    {
        portalNotification.SetActive(false);
    }
}
