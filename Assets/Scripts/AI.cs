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
        if (Input.GetKeyDown(KeyCode.A))
        {
            arSudetosPajegosILista = false;
        }
        if (priesas.arZaidejoEjimas)
        {
            VisosAiPajegos();
            Priesai(priesai, priesas.unit);
            
            //StartCoroutine(PasirenkameUnit(aiPajegos));

        }

    }
    Unit RastiArtimiausiaPriesa(List<Unit> priesai)
    {
        Unit bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = unit.transform.position;
        foreach (Unit priesas in priesai)
        {
            Vector3 directionToTarget = priesas.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = priesas;
            }
        }

        return bestTarget;
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
    private void Priesai(List<Unit> priesai, Unit pasirinktas)
    {
        if (priesai.Count > 0)
        {
            foreach (Unit unit in priesai)
            {
                if (Mathf.Abs(pasirinktas.transform.position.x - unit.transform.position.x) + Mathf.Abs(pasirinktas.transform.position.y - unit.transform.position.y) <= pasirinktas.galimasPultiAtstumas && unit.arPriklausoZaidejui)
                {
                    priesai.Add(unit);
                }
            }
        }
    }
    private Tile ArtimiausiasLangelisEsantisPrieZaidejo(List<Tile> langeliai, Unit unit)
    {
        Tile artimiausiasLangelisPriePrieso = langeliai[0];
        float maziausiasAtstumas = 100;
        foreach (var langelis in langeliai)
        {
            var atstumas = Mathf.Abs(langelis.transform.position.x - artimiausiasLangelisPriePrieso.transform.position.x) + Mathf.Abs(langelis.transform.position.y - artimiausiasLangelisPriePrieso.transform.position.y);

            if (atstumas <= maziausiasAtstumas)
            {
                maziausiasAtstumas = atstumas;
                artimiausiasLangelisPriePrieso = langelis;
            }
        }

        return artimiausiasLangelisPriePrieso;
    }
    private void Puolimas(Unit kasPuola, Unit kaPuola)
    {
        if (kasPuola.arGaliPulti)
        {
            kasPuola.Puolimas(kasPuola, kaPuola);
        }
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
    IEnumerator PasirenkameUnit(List<Unit> kariai)
    {
        
        for (int i = 0; i < kariai.Count; i++)
        {
            
            priesas.unit = kariai[i];
            
            GalimiJudejimoLangeliai(langeliai, priesas.unit);
            judejimoLangelis = ArtimiausiasLangelisEsantisPrieZaidejo(langeliai, priesas.unit);
            if (priesas.unit != null && priesas.unit.arGalimaJudinti == true && priesas.arZaidejoEjimas && priesas.arGalimaJudintiKitaKari)
            {
                priesas.arGalimaJudintiKitaKari = false;
                
                yield return StartCoroutine(judejimoLangelis.PradetiJudejima(priesas, judejimoLangelis));

                

            }
            yield return new WaitForSeconds(2f);
            
        }

        

    }
}
