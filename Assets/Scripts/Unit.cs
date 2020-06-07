using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string pavadinimas;
    public int ataka;
    public int gynybaPriesPestininkus;
    public int gynybaPriesRaitus;
    public float gyvybes;
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
                if (arGalimaJudinti || arGaliPulti && !player.arJauPuole)
                {
                    
                    player.unit = this;
                    
                    
                }
                if (arGalimaJudinti && !player.arJauPuole)
                {
                    GalimiLangeliai();

                }
               
            }
            if (player.unit != null && player.unit.arGaliPulti && !player.arJauPuole)
            { // puolimas
                
                GalimiPuolimoLangeliai();
                GalimiPultiPriesai();
                }
            
            if (player.unit == null)
            {
                Debug.Log("player unit null");
            }
            
          
           
        }
        else
        {
            // Paspaudus ant prieso kariuomenes
            if (player.unit != null && player.arPuolimoFaze && player.unit.arGaliPulti && player.PriesaiEsantysNetoli.Count > 0 && !arPriklausoZaidejui && !player.arJauPuole)
            {
                Debug.Log("puolimas");
                player.arPuolimoFaze = false;
                player.arJauPuole = true;
                player.unit.arGaliPulti = false;
                Puolimas(player.unit, this);
                player.unit = null;
                player.PriesaiEsantysNetoli.Clear();
                gameMaster.IsvalytiPasirinktusLangelius();
                IsvalytiSunaikintoKarioLangeli();
                player.arJauPuole = true;
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
        
        if (kasPuola.tipas.Equals("pestininkas"))
        {
            
            return Ataka(kasPuola.ataka, kaPuola.gynybaPriesPestininkus);
        }
        else if (kasPuola.tipas.Equals("raitininkas"))
        {
            return Ataka(kasPuola.ataka, kaPuola.gynybaPriesRaitus);
        }
        else if (kasPuola.tipas.Equals("magija"))
        {
            return Ataka(kasPuola.ataka, kaPuola.gynybaPriesMagija);
        }
        else
        {
            Debug.Log("Toks puolancio kario tipas neegzistuoja...");
            return 0;
        }
        
    }
   public float Ataka(int kasPuolaAtaka, int gynyba)
    {
        return (kasPuolaAtaka - gynyba) > 0 ? kasPuolaAtaka - gynyba : 0;
    }
    public void Puolimas(Unit kasPuola, Unit kaPuola)
    {
        Debug.Log("Puolimo pradzia");
        // Zaidejas puola priesa
        Debug.Log("Padaryta zala priesui: " + AtakosPaskaiciavimas(kasPuola, kaPuola));
        kaPuola.gyvybes -= AtakosPaskaiciavimas(kasPuola, kaPuola);
        gameMaster.IsvalytiPasirinktusLangelius();
        gameMaster.SunaikintiUnit();
        

    }

    public void IsvalytiSunaikintoKarioLangeli()
    {
        foreach (var langelis in FindObjectsOfType<Tile>())
        {
            langelis.GetComponent<SpriteRenderer>().color = langelis.dabartineSpalva;
            if (langelis.transform.position.x == this.transform.position.x && langelis.transform.position.y == this.transform.position.y)
            {
                langelis.arTusciasLangelis = true;
            }
        }
    }
}
