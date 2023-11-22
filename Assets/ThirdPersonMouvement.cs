using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;



// Inspiré de : https://youtu.be/4HpC--2iowE

public class ThirdPersonMouvement : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [SerializeField] float speed = 6f;
    [SerializeField] Transform cam;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float jumpSpeed = 15;
    [SerializeField] float gravity = -30f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform ground;
    float turnSmoothVelocity;
    Vector3 verticalVelocity = Vector3.zero;
    bool isGrounded = false;
    Animator animator;
    float originalSpeed;
    // Start is called before the first frame update

    private void Awake()
    {
        animator = GetComponent<Animator>();
        originalSpeed= speed;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float halfHeight = controller.height * 0.5f;
        var bottomPoint = transform.TransformPoint(controller.center - Vector3.up * halfHeight);
        isGrounded = Physics.CheckSphere(bottomPoint, 0.1f, groundMask);
        Vector3 moveDir = Vector3.zero;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
        controller.Move(Vector3.zero);

        if (isGrounded)
        {
            animator.SetBool("Grounded", true);
            verticalVelocity.y = 0;
        }
        if (dir.magnitude >= 0.1f /*&& controller.isGrounded*/)
        {
            if( Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("Sprinting", true);
                animator.SetFloat("Speed", 3f);
                speed = 5;
            }
            else
            {
                animator.SetBool("Sprinting", false);
                animator.SetFloat("Speed", 1.1f);
                speed = originalSpeed;
            }
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
            controller.Move(moveDir * speed * Time.deltaTime);
        }
        else
        {
            speed = originalSpeed;
            animator.SetFloat("Speed", 0f);
        }

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity.y = Mathf.Sqrt(-2 * jumpSpeed * gravity);
                animator.SetBool("Grounded", false);
                animator.SetBool("Sprinting", false); ;
            }
        }
        // Gravité
        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);

    }
}
