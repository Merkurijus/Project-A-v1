using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameMaster : MonoBehaviour
{

    #region Visi kintamieji
    public int kiekLangeliuX;
    public int kiekLangeliuY;

    [Header("Spalvos")]
    //Langelių spalvos
    public Color tamsiSpalva;
    public Color sviesiSpalva;
    public Color langelioSpalvaUzvedusPele;
    public Color kariuomenesEjimoSpalva;
    public Color kariuomenesEjimoSpalvaUzvedusPele;
    public Color kariuomenesPuolimoSpalva;
    
    [Header("Tekstu laukai")]
    public Text laikas;
    public Text auksiniai;
    public Text kienoEjimas;
    public TMP_Text kariuText;

    [Header("Karines pajegos prefab")]
    // Kariniu pajegu prefab
    public List<GameObject> kariai;
    public List<Tile> langeliai;
    [Header("UI")]
    public GameObject kariuInfo;
    public GameObject DamageText;
    public GameObject baigtiEjimaMygtukas;
    public GameObject laimetojasUI;
    public TMP_Text laimetojoText;

    private Player zaidejas;
    private Player priesas;

    //Laiko kintamieji
    [Header("Ejimu trukme")]
    public float ejimoLaikas = 30f;
    private float dabartinisLaikas;

    // bool kintamieji
    private bool arZaidimasPrasidejo;
    #endregion
    private void Start()
    {
        zaidejas = GameObject.Find("/Zaidejai/zaidejas").GetComponent<Player>();
        priesas = GameObject.Find("/Zaidejai/priesas").GetComponent<Player>();
        laimetojasUI.SetActive(false);
        PradetiEjima();
        AtnaujintiAuksiniuTeksta();
        GeneruotiAiKariuomene();
    }
    private void Update()
    {
        ZaidimoPradzia();
        if (zaidejas.arZaidejoEjimas) baigtiEjimaMygtukas.SetActive(true);
        else baigtiEjimaMygtukas.SetActive(false);
    }

    #region Zaidimo pradzia, pradedamas laikmatis, atnaujinamas laikas
    void ZaidimoPradzia()
    {
        if (arZaidimasPrasidejo && dabartinisLaikas > 0)
        {
            dabartinisLaikas -= Time.deltaTime;
            AtnaujintiEjimoLaikoTeksta(dabartinisLaikas);
        }
        else
        {
            BaigtiEjima();
        }
    }
    #endregion
    #region Langeliu isvalymai
    public void IsvalytiPasirinktusLangelius()
    {
        foreach (var langelis in FindObjectsOfType<Tile>())
        {
            foreach (var unit in FindObjectsOfType<Unit>())
            {

                langelis.GetComponent<SpriteRenderer>().color = langelis.dabartineSpalva;
                if (langelis.transform.position.x == unit.transform.position.x && langelis.transform.position.y == unit.transform.position.y)
                {
                    langelis.arTusciasLangelis = false;
                }
            }
        }

        
    }
    public void IsvalytiDabartiniLangeli(Vector2 dabartinisLangelis)
    {

        foreach (var langelis in FindObjectsOfType<Tile>())
        {
           

            if (langelis.transform.position.x == dabartinisLangelis.x && langelis.transform.position.y == dabartinisLangelis.y)
            {
                langelis.arTusciasLangelis = true;
            }

            
        }

    }
    #endregion
    #region ejimo pradzia prasidedant zaidimui
    public void PradetiEjima()
    {

            zaidejas.arZaidejoEjimas = true;
            priesas.arZaidejoEjimas = false;
            arZaidimasPrasidejo = true;
            dabartinisLaikas = ejimoLaikas;
            AtnaujintiKienoEjimasVarda();
            zaidejas.arGalimaJudintiKitaKari = true;
            priesas.arGalimaJudintiKitaKari = false;    
    }
    #endregion
    #region Ejimo pabaiga
    public void BaigtiEjima()
    {
            LaimetojoRadimas();
            zaidejas.arZaidejoEjimas = !zaidejas.arZaidejoEjimas;
            priesas.arZaidejoEjimas = !priesas.arZaidejoEjimas;
            dabartinisLaikas = ejimoLaikas;
            AtnaujintiKienoEjimasVarda();
            AtnaujintiKariuLeidimus(zaidejas.arZaidejoEjimas);
            AtnaujintiKariuLeidimus(priesas.arZaidejoEjimas);
            IsvalytiPasirinktusLangelius();
            zaidejas.unit = null;
            zaidejas.arJauPuole = false;
            zaidejas.arGalimaJudintiKitaKari = true;
            priesas.unit = null;
            priesas.arJauPuole = false;
            priesas.arGalimaJudintiKitaKari = true;

    }
    #endregion
    #region Auksiniu, Laiko ir vardo teksto atnaujinimas
    public void AtnaujintiAuksiniuTeksta()
    {
        auksiniai.text = zaidejas.auksiniai.ToString();
    }
    public void AtnaujintiEjimoLaikoTeksta(float dabartinisLaikas)
    {
        laikas.text = Mathf.Round(dabartinisLaikas).ToString();
    }
    public void AtnaujintiKienoEjimasVarda()
    {
        string kienoEjimasText = (zaidejas.arZaidejoEjimas) ? "Žaidėjo ėjimas" : "AI ėjimas";
        kienoEjimas.text = kienoEjimasText;
    }
    #endregion
    #region Tikrinama ar zaidejo ejimas
    public bool arZaidejoEjimas()
    {
        if (zaidejas.arZaidejoEjimas) return true;
        return false;
        
    }
    #endregion
    #region Kariu leidimu atnaujinimai
    public void AtnaujintiKariuLeidimus(bool arZaidejo)
    {
        foreach (var item in FindObjectsOfType<Unit>())
        {
            if (item.arPriklausoZaidejui == arZaidejo)
            {
                item.arGalimaJudinti = true;
                item.arGaliPulti = true;
                item.arPasirinktas = false;
                item.arBaigeJudeti = false;
            }
        }
    }
    #endregion
    #region Kariu naikinimas pasiekus 0 arba maziau gyvybiu
    public void SunaikintiUnit()
    {
        foreach (var item in FindObjectsOfType<GameObject>())
        {
            Unit karys = item.GetComponent<Unit>();
            if (item.GetComponent<Unit>() != null)
            {
                if (karys.gyvybes <= 0)
                {
                    Destroy(item);
                    IsvalytiPasirinktusLangelius();
                }
            }
        }
        
    }
    #endregion
    #region Laimetojo ieskojimas
    public void LaimetojoRadimas()
    {
        int ai = 0;
        int zaidejas = 0;
        foreach (var item in FindObjectsOfType<Unit>())
        {

            if (item.arPriklausoZaidejui)
            {
                zaidejas++;
            }
            else
            {
                ai++;
            }
            
        }
        if (ai == 0)
        {
            laimetojasUI.SetActive(true);
            laimetojoText.text = "Žaidėjas laimėjo";
           
            
        }
        else if(zaidejas == 0)
        {
            laimetojasUI.SetActive(true);
            laimetojoText.text = "AI laimėjo";
           
        }
        Debug.Log(ai);
        Debug.Log(".........");
        Debug.Log(zaidejas);
    }
    #endregion
    #region Generuoti AI kariuomene pagal lygi
    void GeneruotiAiKariuomene()
    {
        decimal lygis = zaidejas.lygis;
        decimal koeficientas = 6;
        var max_kariu = Math.Ceiling(lygis / koeficientas);

        decimal auksiniai = zaidejas.auksiniai;
        decimal auksiniu_koeficientas = 50;
        var max_auksiniu = Math.Ceiling(lygis / koeficientas) * auksiniu_koeficientas;
        zaidejas.auksiniai = (int)max_auksiniu;
        SudedamiLangeliaiILista();
        
        
        for (int i = 0; i < max_kariu; i++)
        {

            int karys = UnityEngine.Random.Range(0, kariai.Count);
            Debug.Log(karys);
            int atsitiktinisLangelis = UnityEngine.Random.Range(0, langeliai.Count);
            var k = Instantiate(kariai[karys]);
            k.transform.position = new Vector3(langeliai[atsitiktinisLangelis].transform.position.x, langeliai[atsitiktinisLangelis].transform.position.y, -5f);
            langeliai[atsitiktinisLangelis].arTusciasLangelis = false;
            langeliai.RemoveAt(atsitiktinisLangelis);

            k.GetComponent<Unit>().arPriklausoZaidejui = false;
            
                
            
            
            
        }
    }
    private void SudedamiLangeliaiILista()
    {

        foreach (var item in FindObjectsOfType<Tile>())
        {
            if (item.transform.position.x >= -2 && item.transform.position.y >= 7 && item.arTusciasLangelis)
            {
                langeliai.Add(item);
            }
            
        }
  
    }
    #endregion
}

