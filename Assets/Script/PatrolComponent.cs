using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class PatrolComponent : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float detectionRange = 3;
    [SerializeField] List<Transform> destinations;
    [SerializeField] float waitTime = 2;

    Animator animator;
    NavMeshAgent agent;
    Node root;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        SetupTree();
    }

    private void SetupTree()
    {
        Node l1 = new IsWithInRage(target, transform, detectionRange);
        Node l2 = new GoToTarget(target, agent, animator);
        Node seq1 = new Sequence(new List<Node>() { l1, l2});
        Node l4 = new Tombe(agent, animator, 8);
        Node seq2 = new Sequence(new List<Node>() { l4 });
        Node l3 = new PatrolTask(destinations, agent, waitTime, animator);
        Node sel1 = new Selector(new List<Node>() { seq1, seq2, l3 });
        
        root = sel1;
    }

    void Update()
    {
        root.Evaluate();
        if (agent.isOnOffMeshLink)
        {
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("Jump", false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerPrefs.SetString("raison", "Vous avez été touché!");

        SceneManager.LoadScene("Menu");
    }
}
