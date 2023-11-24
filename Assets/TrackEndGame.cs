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
        if (Physics.Raycast(transform.position, transform.forward, out objectHit, 2))
        {
            text.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {

                //do something if hit object ie
                if (objectHit.collider.tag == "Player")
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
