using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    [SerializeField] float vitesse = 50;
    [SerializeField] float forceJump = 5;
    [SerializeField] float senssibiliterCamera = 1;
    [SerializeField] float gravity = 1;
    [SerializeField] TextMeshProUGUI temps;
    [SerializeField] Animator animator;
    Vector3 startPostion = Vector3.zero;
    float timeElapsed = 0;

    Vector3 rotationCamera = Vector3.zero;
    Vector3 jump = Vector3.zero;
    CharacterController cc;
    Camera cam;
    public static float Survie;
    // Start is called before the first frame update
    void Start()
    {
        startPostion = transform.position;
        cc = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        timeElapsed += Time.deltaTime;
        if (cc.transform.position.y <= -1)
        {
           print("Tomber");
           cc.transform.position = startPostion;
        }
        else 
        {
            Survie = timeElapsed;
           temps.SetText("" + Survie.ToString("#0.00"));
           Deplacement();
           RotationCamera();
        }
    }
    void RotationCamera()
    {
        Vector3 rotation = Vector3.zero;
        rotationCamera += new Vector3(-Input.GetAxis("Mouse Y") * senssibiliterCamera * Time.deltaTime,
                                    Input.GetAxis("Mouse X") * senssibiliterCamera * Time.deltaTime, 0);
        rotation = rotationCamera;
        rotation.x = Mathf.Clamp(rotation.x, 0f, 0f);
        rotationCamera.x = Mathf.Clamp(rotationCamera.x, -70, 70);
        cam.transform.rotation = Quaternion.Euler(rotationCamera);
        cc.transform.rotation = Quaternion.Euler(rotation);
        //transform.Rotate(transform.right, -Input.GetAxis("Mouse Y") * 3, Space.World);

    }
    void Deplacement()
    {

        
        Vector3 direction = cam.transform.forward * Input.GetAxis("Vertical") + cam.transform.right * Input.GetAxis("Horizontal");

        if (direction.magnitude > 0)
        {
            animator.SetFloat("Speed", 1);
            direction = new Vector3(direction.x, 0, direction.z);
            //direction.y = 0;

            direction.Normalize();
            //direction = direction.normalized;

            direction = direction * (Input.GetAxis("Sprint") * 0.5f + 1);


        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
        if (cc.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                jump = Vector3.up * forceJump;
            }
            jump.y = Mathf.Max(-1, jump.y);
        }
        else
        {
            jump -= Vector3.up * Time.deltaTime * gravity;
        }

        cc.Move((direction * vitesse + jump) * Time.deltaTime);
    }
    
    
}
