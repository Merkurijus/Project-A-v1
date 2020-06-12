using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    Unit unit;
    List <Unit> aiPajegos = new List<Unit>();
    List<Unit> priesai = new List<Unit>();
    List<Tile> langeliai = new List<Tile>();
    private AI ai;
    private Player player;
    private GameMaster gameMaster;
    private bool arSudetosPajegosILista;
    private void Awake()
    {
        
        player = FindObjectOfType<Player>();
        gameMaster = FindObjectOfType<GameMaster>();
        arSudetosPajegosILista = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            arSudetosPajegosILista = false;
        }
        if (!player.arZaidejoEjimas && !arSudetosPajegosILista)
        {

            VisosAiPajegos();
            StartCoroutine(AiJudejimas());
            

            arSudetosPajegosILista = true;
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
        float maziausiasAtstumas = 0;
        foreach (var langelis in langeliai)
        {
            if (Mathf.Abs(langelis.transform.position.x - artimiausiasLangelisPriePrieso.transform.position.x) + Mathf.Abs(langelis.transform.position.y - artimiausiasLangelisPriePrieso.transform.position.y) <= maziausiasAtstumas)
            {
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
        foreach (var item in FindObjectsOfType<Unit>())
        {
            if (!item.arPriklausoZaidejui)
            {
                aiPajegos.Add(item);
            }
        }
    }
    IEnumerator AiJudejimas()
    {
        foreach (var unit in aiPajegos)
        {
            langeliai.Clear();
            GalimiJudejimoLangeliai(langeliai, unit);

            Tile artimiausiasLangelis = ArtimiausiasLangelisEsantisPrieZaidejo(langeliai, unit);
            StartCoroutine(artimiausiasLangelis.PradetiJudejima(unit, artimiausiasLangelis.transform.position));
            gameMaster.IsvalytiPasirinktusLangelius();
            yield return new WaitForSeconds(1f);
            priesai.Clear();
            Priesai(priesai, unit);
            if (priesai.Count> 0)
            {
                Unit kaPulti = RastiArtimiausiaPriesa(priesai);
                Puolimas(unit, kaPulti);
            }
            
            yield return new WaitForSeconds(1f);
        }
        unit = null;
        gameMaster.BaigtiEjima();
        yield return null;
    }
}
