using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string pavadinimas;
    public int ataka;
    public int gynybaPriesPestininkus;
    public int gynybaPriesRaitus;
    public int gyvybes;
    public string tipas;
    public int gynybaPriesMagija;
    public int galimasVaiksciotiAtstumas;
    public int galimasPultiAtstumas;
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
    public bool arBaigeJudeti;
    public float judejimoGreitis;
    
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        gameMaster = FindObjectOfType<GameMaster>();
    }
    private void Update()
    {
        
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
                if (arGalimaJudinti || arGaliPulti)
                {
                    
                    player.unit = this;
                    
                    
                }
                if (arGalimaJudinti)
                {
                    GalimiLangeliai();
                }
               
            }
            if (player.unit != null && player.unit.arGaliPulti)
            { // puolimas
                GalimiPuolimoLangeliai();
                GalimiPultiPriesai();
                player.arPuolimoFaze = true;
                Debug.Log("puolimo faze prasideda");
            }
            if (player.unit == null)
            {
                Debug.Log("player unit null");
            }
            
          
           
        }
        else
        {
            if (player.unit != null && player.arPuolimoFaze && player.unit.arGaliPulti && player.PriesaiEsantysNetoli.Count > 0 && !arPriklausoZaidejui)
            {
                Debug.Log("KIEK PRIESU LISTE: " + player.PriesaiEsantysNetoli.Count);
                //Puolamas priesas
                Debug.Log("x: " + transform.position.x + " y: " + transform.position.y);
                Debug.Log("Ivyko puolimas");
                player.arPuolimoFaze = false;
                player.unit.arGaliPulti = false;
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
    public void GalimiPuolimoLangeliai()
    {
        player.PriesaiEsantysNetoli.Clear();
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
              if (Mathf.Abs(transform.position.x - unit.transform.position.x) + Mathf.Abs(transform.position.y - unit.transform.position.y) <= galimasPultiAtstumas  && !unit.arPriklausoZaidejui)
              {
                      player.PriesaiEsantysNetoli.Add(unit);
              }
        }
    }
    public void GalimiPultiPriesai()
    {
        foreach (var tile in FindObjectsOfType<Tile>())
        {
            foreach (var item in player.PriesaiEsantysNetoli)
            {
                if (tile.transform.position.x == item.transform.position.x && tile.transform.position.y == item.transform.position.y)
                {
                    var col = tile.GetComponent<SpriteRenderer>();
                    col.color = gameMaster.kariuomenesPuolimoSpalva;
                    tile.arAntLangelioEsantiPriesaGalimaPulti = true;
                }
            }
        }
        
    }
    private float AtakosPaskaiciavimas(Unit kasPuola, Unit kaPuola)
    {
        if (kasPuola.tipas.Equals("Pestininkas"))
        {
            return kasPuola.ataka - kaPuola.gynybaPriesPestininkus;
        }
        else if (kasPuola.tipas.Equals("Raitininkas"))
        {
            return kasPuola.ataka - kaPuola.gynybaPriesRaitus;
        }
        else if (kasPuola.tipas.Equals("Magija"))
        {
            return kasPuola.ataka - kaPuola.gynybaPriesMagija;
        }
        else
        {
            Debug.Log("Toks puolancio kario tipas neegzistuoja...");
            return 0;
        }
        
    }
    private float Gyvybes(Unit kasPuola, Unit kaPuola)
    {
        return kaPuola.gyvybes - AtakosPaskaiciavimas(kasPuola, kaPuola);
    }
    public void Puolimas(Unit kasPuola, Unit kaPuola)
    {
        // Zaidejas puola priesa
        Gyvybes(kasPuola, kaPuola);
        // Priesas puola atgal jei tik yra atakos range


    }
    
}
