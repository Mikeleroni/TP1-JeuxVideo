using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerButton : MonoBehaviour
{
    [SerializeField] Transform button;
    [SerializeField] Animator animator;
    ThirdPersonMouvement personMouvement;
    GameObject box;
    
    // Start is called before the first frame update
    private void Awake()
    {
        personMouvement = GetComponent<ThirdPersonMouvement>();
        box = GameObject.FindGameObjectWithTag("Box");
        
    }
    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Vector3.Distance(button.position,transform.position)<=0.8f) 
        {
            if(Input.GetKeyDown(KeyCode.F)) 
            {
                transform.position = new Vector3(-4.27416849f, 1.12999964f, 8.55945587f);
                transform.rotation = Quaternion.Euler( new Vector3(0, 208.563904f, 0));
                //transform.position= new Vector3(-3.8f, 1.13f, 8.4f);
                animator.SetTrigger("Triggered");
                personMouvement.enabled = false;
                Debug.Log("Yes");
                Renderer renderer = box.GetComponent<Renderer>();
                if (renderer != null)
                {
                    // Si l'objet a un Renderer, changer sa couleur
                    Material material = renderer.material;
                    if(material.color != Color.red)
                    {
                        material.color = Color.red;
                    }
                    else
                    {
                        material.color = Color.white;
                    }
                    
                }
            }
        }
    }
    public void AlertAnimationEnd()
    {
        personMouvement.enabled = true;
    }
}