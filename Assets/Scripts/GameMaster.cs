using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameMaster : MonoBehaviour
{
   

    public int kiekLangeliuX;
    public int kiekLangeliuY;


    //Langelių spalvos
    public Color tamsiSpalva;
    public Color sviesiSpalva;
    public Color langelioSpalvaUzvedusPele;
    public Color kariuomenesEjimoSpalva;
    public Color kariuomenesEjimoSpalvaUzvedusPele;
    public Color kariuomenesPuolimoSpalva;
    

    public Text laikas;
    public Text auksiniai;
    public Text kienoEjimas;

    // Kariuomeniu prefab
    public GameObject pestininkas;

    private Player zaidejas;
    private Player priesas;
    public float ejimoLaikas = 30f;
    private float dabartinisLaikas;
    private bool arZaidimasPrasidejo;

    private void Start()
    {
        zaidejas = GameObject.Find("/Zaidejai/zaidejas").GetComponent<Player>();
        priesas = GameObject.Find("/Zaidejai/priesas").GetComponent<Player>();
        PradetiEjima();
        AtnaujintiAuksiniuTeksta();
    }
    private void Update()
    {

        ZaidimoPradzia();
       
    }
    void ZaidimoPradzia()
    {
        if (arZaidimasPrasidejo && dabartinisLaikas > 0)
        {
            dabartinisLaikas -= Time.deltaTime;
            AtnaujintiEjimoLaikoTeksta(dabartinisLaikas);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                BaigtiEjima();
            }
        }
        else
        {
            BaigtiEjima();
        }
    }
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
    public void BaigtiEjima()
    {
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
        Debug.Log("ar zaidejo ejimas" + zaidejas.arZaidejoEjimas);
        Debug.Log("ar prieso ejimas" + priesas.arZaidejoEjimas);
    }
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
    public bool arZaidejoEjimas()
    {
        if (zaidejas.arZaidejoEjimas) return true;
        return false;
        
    }
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
}

