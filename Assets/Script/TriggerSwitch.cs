using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using TMPro;


public class TriggerSwitch : MonoBehaviour
{
    [SerializeField] Transform transformCamera;
    [SerializeField] float maxDistance;
    [SerializeField] GameObject text;
    bool triggered;

    void Start()
    {
    }
    public static string raison = "";
    void Update()
    {
        int layerMask = LayerMask.GetMask("Interrupteur");
        RaycastHit hit;
        if((Physics.Raycast(transformCamera.position, transformCamera.TransformDirection(Vector3.forward), out hit, maxDistance, layerMask)))
        {
            text.SetActive(true);
            Debug.Log("lol");
            if (Input.GetKeyDown(KeyCode.F))
            {
             PlayerPrefs.SetString("raison", "Vous avez gagnée!");
             SceneManager.LoadScene("Menu");
            }
        }
        else
        {
            text.SetActive(false);
        }
    }
}
