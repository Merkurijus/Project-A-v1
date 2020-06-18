using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LygioTopuSarasas : MonoBehaviour
{
    string vardas;
    public GameObject Sarasas;
    public GameObject sarasoElementas;
    void Start()
    {
        vardas = PlayerPrefs.GetString("vardas", "nera");
        Debug.Log(vardas);
        Formos forma = new Formos();
        StartCoroutine(forma.LygioTopai(vardas));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
