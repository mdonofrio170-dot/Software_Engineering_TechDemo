using System;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private float rotationSpeed = 720f;

    [SerializeField]
    public float jumpSpeed = 5f;

    [SerializeField]
    private float jumpHorizontalSpeed = 5f;

    [SerializeField]
    private float jumpGracePeriod = 0.2f;

    private float ySpeed;
    private float oldStepOffset;
    private float? lastGroundedTime;
    private float? jumpPressedTime;
    private bool isJumping;
    private bool isGrounded;

    private CharacterController characterController;


    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        oldStepOffset = characterController.stepOffset;
    }
    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDir = new Vector3(horizontalInput, 0, verticalInput);


        movementDir.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpPressedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpGracePeriod)
        {
            characterController.stepOffset = oldStepOffset;
            ySpeed = -0.5f;
            animator.SetBool("isGrounded", true);
            isGrounded = true;
            animator.SetBool("isJumping", false);
            isJumping = false;
            animator.SetBool("isFalling", false);
            if (Time.time - jumpPressedTime <= jumpGracePeriod)
            {
                ySpeed = jumpSpeed;
                animator.SetBool("isJumping", true);
                isJumping = true;
                jumpPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0.0f;
            animator.SetBool("isGrounded", false);
            isGrounded = false;

            if ((isJumping && ySpeed < 0) || ySpeed < -2)
            {
                animator.SetBool("isFalling", true);
            }
        }



        if (movementDir != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
            Quaternion toRotation = Quaternion.LookRotation(movementDir, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (isGrounded == false)
        {
            Vector3 velocity = movementDir * jumpHorizontalSpeed;
            velocity.y = ySpeed;

            characterController.Move(velocity * Time.deltaTime);
        }

    }

    public void SetWorking(bool value)
    {
        animator.SetBool("Working", value);
    }

    private void OnAnimatorMove()
    {
        if (isGrounded)
        {
            Vector3 velocity = animator.deltaPosition;
            velocity.y = ySpeed * Time.deltaTime;
            characterController.Move(velocity);
        }

    }
}