using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Ennemi : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject playerRef;
    CapsuleCollider capCollider;
    // Start is called before the first frame update
    void Start()
    {
        capCollider = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        playerRef = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = playerRef.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerPrefs.SetString("raison", "Vous avez été touché!");

        SceneManager.LoadScene("Menu");
    }
}
