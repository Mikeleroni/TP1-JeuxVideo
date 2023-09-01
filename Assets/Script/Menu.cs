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
        temps.SetText("Temps: " + Player.Survie.ToString("#0.00"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
