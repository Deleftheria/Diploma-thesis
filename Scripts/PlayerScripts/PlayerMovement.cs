using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;
    public float jumpSpeed;
    public float jumpButtonGracePeriod;

    [SerializeField]
    private Transform cameraTransform;

    private Animator animator;
    private CharacterController characterController;
    private float yDirectionSpeed;
    private float stepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    SaveNLoad playerPosData;
    private void Awake()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.startingZoneBGMusic);

        Debug.Log("Load game");
        playerPosData = FindObjectOfType<SaveNLoad>();
        playerPosData.Load();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        stepOffset = characterController.stepOffset;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        float magnitude = Mathf.Clamp01(moveDirection.magnitude) * moveSpeed;

        moveDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * moveDirection;
        moveDirection.Normalize();

        yDirectionSpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = stepOffset;
            yDirectionSpeed = -0.5f;

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                yDirectionSpeed = jumpSpeed;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else 
        {
            characterController.stepOffset = 0;
        }

        Vector3 velocity = moveDirection * magnitude;
        velocity.y = yDirectionSpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
            Quaternion rotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            //play the sound
            SoundManager.Instance.PlaySound(SoundManager.Instance.grassWalkSound);
        }
        else
        {
            animator.SetBool("isMoving", false);

            //stop the sound
            SoundManager.Instance.grassWalkSound.Stop();
        }
    }
}
