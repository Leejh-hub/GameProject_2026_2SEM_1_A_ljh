using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;         

    [SerializeField] private Transform cameraTrasform;

    [Header("이동 설정")]

    [SerializeField] private float walkSpeed = 3f;

    [SerializeField] private float runSpeed = 6f;

    [SerializeField] private float rotationSpeed = 10f;

    [Header("바닥 설정")]

    [SerializeField] private float Gravity = -20f;

    private CharacterController controller;

    private float verticalvelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //1.WASD 입력

        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
        {
            return;
        }

        Vector2 input = Vector2.zero;

        if (keyboard.aKey.isPressed)
            input.x -= 1f;
        if (keyboard.dKey.isPressed)
            input.x += 1f;
        if (keyboard.sKey.isPressed)
            input.y -= 1f;
        if (keyboard.wKey.isPressed)
            input.y += 1f;


        input = Vector2.ClampMagnitude(input, 1f);

        //2. 
        Vector3 cameraForward = cameraTrasform.forward;     
        Vector3 cameraRight = cameraTrasform.right;

        //

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        //3.

        Vector3 moveDirection = cameraForward * input.y + cameraRight * input.x;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        //4.

        bool isRunning = keyboard.leftShiftKey.isPressed;
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        //5.

        controller.Move(moveDirection * currentSpeed * Time.deltaTime);

        //6

        if(moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        //7.
            
        if(controller.isGrounded && verticalvelocity < 0f)
        {
            verticalvelocity = -2f; 
        }
        else
        {
            verticalvelocity += Gravity * Time.deltaTime;
        }

        controller.Move(Vector3.up * verticalvelocity * Time.deltaTime);

        //8

        float animationSpeed = 0f;

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            animationSpeed = isRunning ? 1f : 0.5f; 
        }

        animator.SetFloat("speed", animationSpeed, 0.1f, Time.deltaTime );
    }
}
