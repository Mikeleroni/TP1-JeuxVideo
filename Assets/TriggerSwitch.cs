using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerSwitch : MonoBehaviour
{
    [SerializeField] Transform transformCamera;
    [SerializeField] float maxDistance;
    bool triggered;

    void Start()
    {

    }

    void Update()
    {
            int layerMask = LayerMask.GetMask("Interrupteur");
            RaycastHit hit;
            if (Physics.Raycast(transformCamera.position, transformCamera.TransformDirection(Vector3.forward), out hit, maxDistance, layerMask) && Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Hit");
            }
    }
}
