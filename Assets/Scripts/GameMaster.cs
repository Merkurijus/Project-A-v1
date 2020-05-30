using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField]
 
    public GameObject paryskintasLangelis;

    public int kiekLangeliuX;
    public int kiekLangeliuY;


    //Langelių spalvos
    public Color tamsiSpalva;
    public Color sviesiSpalva;
    public Color langelioSpalvaUzvedusPele;
    public Color kariuomenesEjimoSpalva;
    public Color kariuomenesEjimoSpalvaUzvedusPele;

    // Kariuomeniu prefab
    public GameObject pestininkas;

    

    private void Start()
    {
       
    }
    public void IsvalytiPasirinktusLangelius()
    {
        foreach (var langelis in FindObjectsOfType<Tile>())
        {
            langelis.GetComponent<SpriteRenderer>().color = langelis.dabartineSpalva;
        }
    }
}

