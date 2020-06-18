using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [Header("string tipo kintamieji")]
    public string pavadinimas;
    public string tipas;

    [Header("int tipo kintamieji")]
    public int ataka;
    public int gynybaPriesPestininkus;
    public int gynybaPriesRaitus;
    public int gynybaPriesMagija;
    public int galimasVaiksciotiAtstumas;
    public int galimasPultiAtstumas;

    [Header("float tipo kintamieji")]
    public float judejimoGreitis;
    public float gyvybes;

    [Header("bool tipo kintamieji")]
    public bool arGalimaJudinti;
    public bool arGaliPulti;
    public bool arPasirinktas;
    public bool arYraArenoje;
    public bool arPriklausoZaidejui;
    public bool arBaigeJudeti;


    public GameObject puolimoLangelis;
    public GameObject ejimoLangelis;

    private PridetiKariuomeneUI UI;
    private Player zaidejas;
    private GameMaster gameMaster;
    private void Awake()
    {
        zaidejas = GameObject.Find("/Zaidejai/zaidejas").GetComponent<Player>();
        gameMaster = FindObjectOfType<GameMaster>();
        UI = FindObjectOfType<PridetiKariuomeneUI>();
        puolimoLangelis.SetActive(false);
        ejimoLangelis.SetActive(false);
    }
    private void Update()
    {
        
        if (arPriklausoZaidejui || zaidejas.unit == null) ejimoLangelis.SetActive(true);
        else ejimoLangelis.SetActive(false);

        if (arPriklausoZaidejui && !arGalimaJudinti) puolimoLangelis.SetActive(true);
        else puolimoLangelis.SetActive(false);

        if (!arPriklausoZaidejui)
        {
            puolimoLangelis.SetActive(false);
            ejimoLangelis.SetActive(false);
        }

    }
    private void OnMouseDown()
    {
        gameMaster.kariuInfo.SetActive(true);
        KariuInfo();
        gameMaster.IsvalytiPasirinktusLangelius();
        if (gameMaster.arZaidejoEjimas() && arPriklausoZaidejui && zaidejas.arGalimaJudintiKitaKari)
        {
            arPasirinktas = !arPasirinktas;
            if (!arPasirinktas)
            {
                zaidejas.unit = null;
            }
            else
            {
                if (arGalimaJudinti || arGaliPulti && !zaidejas.arJauPuole)
                {
                    zaidejas.unit = this;
                }
                if (arGalimaJudinti && !zaidejas.arJauPuole)
                {
                    GalimiLangeliai();
                } 
            }
            if (zaidejas.unit != null && zaidejas.unit.arGaliPulti && !zaidejas.arJauPuole && arBaigeJudeti)
            {
                GalimiPuolimoLangeliai();
                GalimiPultiPriesai();
            }
        }
        else
        {
            SpaudziamaPultiPrisa();
        }
        
       
       

    }
    void SpaudziamaPultiPrisa()
    {
        // Paspaudus ant prieso kariuomenes
        if (zaidejas.unit != null && zaidejas.arPuolimoFaze && zaidejas.unit.arGaliPulti && zaidejas.PriesaiEsantysNetoli.Count > 0 && !arPriklausoZaidejui && !zaidejas.arJauPuole)
        {
            zaidejas.arPuolimoFaze = false;
            zaidejas.arJauPuole = true;
            zaidejas.unit.arGaliPulti = false;
            Puolimas(zaidejas.unit, this, zaidejas);
            zaidejas.unit = null;
            zaidejas.PriesaiEsantysNetoli.Clear();
            IsvalytiSunaikintoKarioLangeli();
            zaidejas.arJauPuole = true;
        }
    }
    private void KariuInfo()
    {
        string tekstas = pavadinimas + "<br>" + " Ataka: " + ataka + "<br> Gynyba prieš pėstininkus: " + gynybaPriesPestininkus + "<br> Gynyba prieš Raitelius: " + gynybaPriesRaitus + "<br> Gynyba prieš magus: " + gynybaPriesMagija + "<br> Judėjimo atstumas: " + galimasVaiksciotiAtstumas + "<br> Puolimo atstumas: " + galimasPultiAtstumas +"<br> Gyvybių: " + gyvybes;

        gameMaster.kariuText.text = tekstas;
    }
    private void GalimiLangeliai()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (this.transform.position.x == tile.transform.position.x && this.transform.position.y == tile.transform.position.y)
            {
                zaidejas.dabartinisLangelis = tile;
                
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
        zaidejas.PriesaiEsantysNetoli.Clear();
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
              if (Mathf.Abs(transform.position.x - unit.transform.position.x) + Mathf.Abs(transform.position.y - unit.transform.position.y) <= galimasPultiAtstumas  && !unit.arPriklausoZaidejui)
              {
                      zaidejas.PriesaiEsantysNetoli.Add(unit);
              }
        }
    }
    public void GalimiPultiPriesai()
    {
        foreach (var tile in FindObjectsOfType<Tile>())
        {
            foreach (var item in zaidejas.PriesaiEsantysNetoli)
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
    public void Puolimas(Unit kasPuola, Unit kaPuola, Player zaidejas)
    {
       
        kaPuola.gyvybes -= AtakosPaskaiciavimas(kasPuola, kaPuola);
        zaidejas.arJauPuole = true;
        ZalosTekstas(kasPuola, kaPuola);
        gameMaster.SunaikintiUnit();
        IsvalytiSunaikintoKarioLangeli();
        
    }
    public void ZalosTekstas(Unit kasPuola, Unit kaPuola)
    {
        var dmg = Instantiate(gameMaster.DamageText, UI.transform);
        dmg.transform.position = kaPuola.transform.position;
        dmg.GetComponent<TMP_Text>().text = (AtakosPaskaiciavimas(kasPuola, kaPuola)).ToString();
        Destroy(dmg, 1f);
    }
    public void IsvalytiSunaikintoKarioLangeli()
    {
       
        foreach (var langelis in FindObjectsOfType<Tile>())
        {
            langelis.GetComponent<SpriteRenderer>().color = langelis.dabartineSpalva;
            if (langelis.transform.position.x == this.transform.position.x && langelis.transform.position.y == this.transform.position.y && this.gyvybes <= 0)
            {
                langelis.arTusciasLangelis = true;
            }
            
        }
    }
}
