using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private GameMaster gameMaster;
    private Player player;
    private PridetiKariuomeneUI pridetiKariuomeneUI;
    private SpriteRenderer sprite;
    public Color dabartineSpalva;
    public bool arPasirinktas;
    private bool galimasEjimas;
    public bool arTusciasLangelis = true;
    public bool arAntLangelioEsantiPriesaGalimaPulti;
    private void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        player = FindObjectOfType<Player>();
        sprite = GetComponent<SpriteRenderer>();
        dabartineSpalva = sprite.color;
        pridetiKariuomeneUI = FindObjectOfType<PridetiKariuomeneUI>();
    }
    private void OnMouseDown()
    {

        PadedamasKarys();
        PaspaustaEiti();
    }
    private void OnMouseOver()
    {
        UzvedamiLangeliai(gameMaster.langelioSpalvaUzvedusPele, gameMaster.kariuomenesEjimoSpalvaUzvedusPele, gameMaster.kariuomenesPuolimoSpalva, gameMaster.kariuomenesEjimoSpalva);
    }
    private void OnMouseExit()
    {
        UzvedamiLangeliai(dabartineSpalva, gameMaster.kariuomenesEjimoSpalva, gameMaster.kariuomenesEjimoSpalva, dabartineSpalva);
    }
    void UzvedamiLangeliai(Color dabartineSpalva, Color galimoEjimoSpalva, Color galimoPuolimoSpalva, Color dedamoKarioSpalva)
    {
        if (player.unit == null)
        {
            sprite.color = dabartineSpalva;
        }
        else if (GalimasEjimas() && player.arZaidejoEjimas)
        {
            sprite.color = galimoEjimoSpalva;
        }
        
        if (player.arKarysRankoje && arTusciasLangelis && transform.position.y <= 3 && player.arZaidejoEjimas)
        {
            sprite.color = dedamoKarioSpalva;
        }
    }
    void PadedamasKarys()
    {
        if (player.rankojeUnit != null && arTusciasLangelis && transform.position.y <=3)
        {
            var u = Instantiate(gameMaster.pestininkas);
            u.transform.position = this.transform.position;
            u.transform.position = new Vector3(u.transform.position.x, u.transform.position.y, -5f);
            var unitClass = u.GetComponent<Unit>();
            unitClass.arPriklausoZaidejui = true;
            player.rankojeUnit = null;
            player.unit = null;
            player.arKarysRankoje = false;
            arTusciasLangelis = false;
            pridetiKariuomeneUI.pirktiKariUI.SetActive(true);
        }
    }
    private bool GalimasEjimas()
    {

        if (player.unit != null && Mathf.Abs(player.unit.transform.position.x - this.transform.position.x) + Mathf.Abs(player.unit.transform.position.y - this.transform.position.y) <= player.unit.galimasVaiksciotiAtstumas && !player.arJauPuole)
        {
            if (this.arTusciasLangelis && player.unit.arGalimaJudinti)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }
    private bool GalimasPuolimas()
    {
        if (player.unit != null && Mathf.Abs(player.unit.transform.position.x - this.transform.position.x) + Mathf.Abs(player.unit.transform.position.y - this.transform.position.y) <= player.unit.galimasPultiAtstumas)
        {
            if (!this.arTusciasLangelis)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    void PaspaustaEiti()
    {

        if (GalimasEjimas() && player.unit != null && player.unit.arGalimaJudinti == true && arTusciasLangelis && player.arZaidejoEjimas)
        {
            StartCoroutine(PradetiJudejima(player.unit, transform.position));
            Judejimas(player.unit);
        }
    }
    public IEnumerator PradetiJudejima(Unit unit, Vector3 kur)
    {
        while (unit.transform.position.x != kur.x)
        {
            unit.transform.position = Vector2.MoveTowards(unit.transform.position, new Vector2(kur.x, unit.transform.position.y), unit.judejimoGreitis * Time.deltaTime);
            yield return null;
        }
        while (unit.transform.position.y != kur.y)
        {
            unit.transform.position = Vector2.MoveTowards(unit.transform.position, new Vector2(unit.transform.position.x, kur.y), unit.judejimoGreitis * Time.deltaTime);
            yield return null;
        }
        
    }
    private void Judejimas(Unit unit)
    {
        unit.arBaigeJudeti = true;
        unit.transform.position = new Vector3(unit.transform.position.x, unit.transform.position.y, -5f);
        unit.arGalimaJudinti = false;
        unit.arGaliPulti = true;



        unit.GalimiPuolimoLangeliai();

        player.arPuolimoFaze = true;
        gameMaster.IsvalytiPasirinktusLangelius();
        unit.GalimiPultiPriesai();

        if (player.dabartinisLangelis != null)
        {
            player.dabartinisLangelis.arTusciasLangelis = true;
            player.dabartinisLangelis = this;
        }

    }
}
