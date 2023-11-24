using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TrackEndGame : MonoBehaviour
{
    [SerializeField] GameObject text;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit objectHit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, fwd, Color.blue);
        if (Physics.Raycast(transform.position, transform.forward, out objectHit, 0.5f))
        {
            if (objectHit.collider.tag == "Player")
            {
                text.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PlayerPrefs.SetString("raison", "Vous avez gagnée!");
                    SceneManager.LoadScene("Menu");
                }

            }
        }
        else
        {
            text.SetActive(false);
        }
    }

}
