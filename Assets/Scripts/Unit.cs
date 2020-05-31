using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string pavadinimas;
    public int ataka;
    public int gynybaPriesPestininkus;
    public int gynybaPriesRaitus;
    public string tipas;
    public int gynybaPriesMagija;
    public int galimasVaiksciotiAtstumas;

    // Uzdeti ginklai ir sarvai
    private Ginklai ginklas;
    private Sarvai sarvai;
    private Player player;
    private GameMaster gameMaster;
    public bool arGalimaJudinti;
    public bool arGaliPulti;
    public bool arPasirinktas;
    public bool arYraArenoje;
    public bool arPriklausoZaidejui;

    public float judejimoGreitis;
    
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        gameMaster = FindObjectOfType<GameMaster>();
    }
    private void OnMouseDown()
    {
        gameMaster.IsvalytiPasirinktusLangelius();
        if (gameMaster.arZaidejoEjimas() && arPriklausoZaidejui)
        {

            arPasirinktas = !arPasirinktas;
            if (!arPasirinktas)
            {
                player.unit = null;
            }
            else
            {
                if (arGalimaJudinti)
                {
                    
                    player.unit = this;
                    GalimiLangeliai();
                }

            }
        }
        
       
       

    }
    private void GalimiLangeliai()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (this.transform.position.x == tile.transform.position.x && this.transform.position.y == tile.transform.position.y)
            {
                player.dabartinisLangelis = tile;
               
            }

            if (Mathf.Abs(transform.position.x - tile.transform.position.x) + Mathf.Abs(transform.position.y - tile.transform.position.y) <= galimasVaiksciotiAtstumas && tile.arTusciasLangelis)
            {
                var col = tile.GetComponent<SpriteRenderer>();
                col.color = gameMaster.kariuomenesEjimoSpalva;
               
            }
        }
    }
}
