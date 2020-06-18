using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    Unit unit;
    List <Unit> aiPajegos = new List<Unit>();
    List<Unit> priesai = new List<Unit>();
    List<Tile> langeliai = new List<Tile>();
    Tile judejimoLangelis;
    private Player zaidejas;
    private Player priesas;
    private GameMaster gameMaster;
    private bool arSudetosPajegosILista;
    bool arJauPuole = false;
    bool arBaigtiEjima = true;
    private void Awake()
    {

        priesas = GameObject.Find("/Zaidejai/priesas").GetComponent<Player>();
        zaidejas = GameObject.Find("/Zaidejai/zaidejas").GetComponent<Player>();
        gameMaster = FindObjectOfType<GameMaster>();
        
    }
    private void Update()
    {
        if (priesas.arZaidejoEjimas)
        {
            
            VisosAiPajegos();
            PasirenkameUnit(aiPajegos);
            
        }

    }
   
  
     
    private void GalimiJudejimoLangeliai(List<Tile> langeliai, Unit unit)
    {
        langeliai.Clear();
        foreach (var tile in FindObjectsOfType<Tile>())
        {
            
            if (Mathf.Abs(unit.transform.position.x - tile.transform.position.x) + Mathf.Abs(unit.transform.position.y - tile.transform.position.y) <= unit.galimasVaiksciotiAtstumas && tile.arTusciasLangelis)
            {
                var col = tile.GetComponent<SpriteRenderer>();
                col.color = gameMaster.kariuomenesEjimoSpalva;
                
                langeliai.Add(tile);
            }
        }
       
    }
    private List<Unit> GalimiPultiPriesai(List<Unit> priesai, Unit unit)
    {
        priesai.Clear();
        foreach (var priesas in FindObjectsOfType<Unit>())
        {

            if (Mathf.Abs(priesas.transform.position.x - unit.transform.position.x) + Mathf.Abs(priesas.transform.position.y - unit.transform.position.y) <= unit.galimasPultiAtstumas && priesas.arPriklausoZaidejui)
            {
               
                priesai.Add(priesas);
            }
        }
        return priesai;
    }
    private Tile AtsitiktinisJudejimoLangelis(List<Tile> langeliai)
    {

        int rand = Random.Range(0, langeliai.Count);

        return langeliai[rand];
    }
 
    private void VisosAiPajegos()
    {
        aiPajegos.Clear();
        foreach (var item in FindObjectsOfType<Unit>())
        {
            if (!item.arPriklausoZaidejui)
            {
                aiPajegos.Add(item);
            }
        }
    }
    void PasirenkameUnit(List<Unit> kariai)
    {
        bool arJauPuole = false;
       
        if (priesas.arZaidejoEjimas)
        {

            gameMaster.IsvalytiPasirinktusLangelius();
            for (int i = 0; i < kariai.Count; i++)
            {
                if (!arJauPuole)
                {
                    priesas.unit = kariai[i];
                    Vector2 dabartinisLangelis = kariai[i].transform.position;

                    GalimiJudejimoLangeliai(langeliai, priesas.unit);
                    judejimoLangelis = AtsitiktinisJudejimoLangelis(langeliai);
                    gameMaster.IsvalytiDabartiniLangeli(dabartinisLangelis);


                    priesas.unit.transform.position = judejimoLangelis.transform.position;
                    priesas.unit.transform.position = new Vector3(priesas.unit.transform.position.x, priesas.unit.transform.position.y, -5f);

                    judejimoLangelis.arTusciasLangelis = false;
                    priesai = GalimiPultiPriesai(priesai, priesas.unit);

                    if (priesai.Count > 0)
                    {

                        priesas.unit.Puolimas(priesas.unit, priesai[0], priesas);
                        arJauPuole = true;
                        gameMaster.BaigtiEjima();
                        break;
                    }



                }
            }
            if (!arJauPuole)
            {
                gameMaster.BaigtiEjima();
            }
             
            
            
        }

    }
}
