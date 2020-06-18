using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private GameMaster gameMaster;
    private Player zaidejas;
    private Player priesas;
    private PridetiKariuomeneUI pridetiKariuomeneUI;
    private SpriteRenderer sprite;

    [Header("Dabartine langelio spalva")]
    public Color dabartineSpalva;

    [Header("Bool kintamieji")]
    public bool arPasirinktas;
    public bool arTusciasLangelis = true;
    public bool arAntLangelioEsantiPriesaGalimaPulti;

    private void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        zaidejas = GameObject.Find("/Zaidejai/zaidejas").GetComponent<Player>();
        priesas = GameObject.Find("/Zaidejai/priesas").GetComponent<Player>();
        sprite = GetComponent<SpriteRenderer>();
        dabartineSpalva = sprite.color;
        pridetiKariuomeneUI = FindObjectOfType<PridetiKariuomeneUI>();
    }
    private void OnMouseDown()
    {
        PadedamasKarys(zaidejas);
        PaspaustaEiti(zaidejas, this);
    }
    #region Langelio spalva uzvedus ir patraukus pele
    private void OnMouseOver()
    {
        UzvedamiLangeliai(gameMaster.langelioSpalvaUzvedusPele, gameMaster.kariuomenesEjimoSpalvaUzvedusPele, gameMaster.kariuomenesPuolimoSpalva, gameMaster.kariuomenesEjimoSpalva);
    }
    private void OnMouseExit()
    {
        UzvedamiLangeliai(dabartineSpalva, gameMaster.kariuomenesEjimoSpalva, gameMaster.kariuomenesEjimoSpalva, dabartineSpalva);
    }
    #endregion
    #region Uzvedamu langeliu spalvos
    void UzvedamiLangeliai(Color dabartineSpalva, Color galimoEjimoSpalva, Color galimoPuolimoSpalva, Color dedamoKarioSpalva)
    {
        if (zaidejas.unit == null)
        {
            sprite.color = dabartineSpalva;
        }
        else if (GalimasEjimas(zaidejas) && zaidejas.arZaidejoEjimas)
        {
            sprite.color = galimoEjimoSpalva;
        }
        
        if (zaidejas.arKarysRankoje && arTusciasLangelis && transform.position.y <= 3 && zaidejas.arZaidejoEjimas)
        {
            sprite.color = dedamoKarioSpalva;
        }
    }
    #endregion
    #region Padedamas karys paspaudus ant langelio
    void PadedamasKarys(Player player)
    {
        if (player.rankoje != null && arTusciasLangelis && transform.position.y <=3)
        {
            var u = Instantiate(zaidejas.rankoje);
            u.transform.position = this.transform.position;
            u.transform.position = new Vector3(u.transform.position.x, u.transform.position.y, -5f);
            var unitClass = u.GetComponent<Unit>();
            unitClass.arPriklausoZaidejui = true;
            zaidejas.rankoje = null;
            player.unit = null;
            player.arKarysRankoje = false;
            arTusciasLangelis = false;
            pridetiKariuomeneUI.pirktiPestininkaUI.SetActive(true);
            pridetiKariuomeneUI.pirktiLankininkaUI.SetActive(true);
            pridetiKariuomeneUI.pirktiMagaUI.SetActive(true);
        }
    }
    #endregion
    #region Galimas ejimas
    private bool GalimasEjimas(Player player)
    {
        if (player.unit != null && Mathf.Abs(player.unit.transform.position.x - this.transform.position.x) + Mathf.Abs(player.unit.transform.position.y - this.transform.position.y) <= player.unit.galimasVaiksciotiAtstumas && !player.arJauPuole)
        {
            if (this.arTusciasLangelis && player.unit.arGalimaJudinti) return true;
            else return false;
        }
        else
        {
            return false;
        }

    }
    #endregion
    #region Kariuomenes judejimas paspaudus ant langelio
    public void PaspaustaEiti(Player player, Tile tile)
    {
        if (GalimasEjimas(player) && player.unit != null && player.unit.arGalimaJudinti == true && tile.arTusciasLangelis && player.arZaidejoEjimas && player.arGalimaJudintiKitaKari)
        {
            player.arGalimaJudintiKitaKari = false;
            StartCoroutine(PradetiJudejima(player, this));
        }
    }
    #endregion
    #region Kariuomenes judejimas
    public IEnumerator PradetiJudejima(Player player, Tile kur)
    {
        
        while (player.unit.transform.position.x != kur.transform.position.x)
        {
            player.unit.transform.position = Vector2.MoveTowards(player.unit.transform.position, new Vector2(kur.transform.position.x, player.unit.transform.position.y), player.unit.judejimoGreitis * Time.deltaTime);
            yield return null;
        }
        while (player.unit.transform.position.y != kur.transform.position.y)
        {
            player.unit.transform.position = Vector2.MoveTowards(player.unit.transform.position, new Vector2(player.unit.transform.position.x, kur.transform.position.y), player.unit.judejimoGreitis * Time.deltaTime);
            yield return null;
        }
       
        if (ArKarysBaigeJudeti(player, this)) Judejimas(player);

        yield return new WaitForSeconds(2f);
    }
    #endregion
    #region Tikrinama ar karys baige judeti
    private bool ArKarysBaigeJudeti(Player player, Tile tile)
    {
        if (player.unit.transform.position.x == tile.transform.position.x && player.unit.transform.position.y == player.unit.transform.position.y) return true;
        return false;
    }
    #endregion
    #region Judinamas karys ir atnaujinami zaidejo kintamieji 
    private void Judejimas(Player player)
    {
        player.unit.arBaigeJudeti = true;
        player.unit.transform.position = new Vector3(player.unit.transform.position.x, player.unit.transform.position.y, -5f);
        player.unit.arGalimaJudinti = false;
        player.unit.arGaliPulti = true;
        player.arGalimaJudintiKitaKari = true;
        player.unit.GalimiPuolimoLangeliai();

        player.arPuolimoFaze = true;
        
        gameMaster.IsvalytiPasirinktusLangelius();
        player.unit.GalimiPultiPriesai();

        if (player.dabartinisLangelis != null)
        {
            player.dabartinisLangelis.arTusciasLangelis = true;
            player.dabartinisLangelis = this;
        }
        
    }
    #endregion
}
