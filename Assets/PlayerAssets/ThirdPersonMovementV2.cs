using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovementV2 : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] float speed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal,0f,vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
           float targetAngle = Mathf.Atan2(direction.x,direction.y) *Mathf.Rad2Deg;
           transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
           characterController.Move(direction*speed*Time.deltaTime);
        }
    }
}
