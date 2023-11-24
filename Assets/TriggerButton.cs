using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerButton : MonoBehaviour
{
    [SerializeField] Transform button;
    [SerializeField] Animator animator;
    ThirdPersonMouvement personMouvement;
    
    // Start is called before the first frame update
    private void Awake()
    {
        personMouvement = GetComponent<ThirdPersonMouvement>();
    }
    void Start()
    {

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
            }
        }
    }
    public void AlertAnimationEnd()
    {
        personMouvement.enabled = true;
    }
}
