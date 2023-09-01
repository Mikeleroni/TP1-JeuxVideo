using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textToucher;
    [SerializeField] TextMeshProUGUI temps;
    // Start is called before the first frame update
    void Start()
    {
        textToucher.SetText(Ennemi.raison);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
