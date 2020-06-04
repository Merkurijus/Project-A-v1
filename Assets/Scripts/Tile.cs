using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private GameMaster gameMaster;
    private Player player;

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
    }
    private void OnMouseDown()
    {

        if (player.rankojeUnit != null && arTusciasLangelis)
        {
            var u = Instantiate(gameMaster.pestininkas);
            u.transform.position = this.transform.position;
            u.transform.position = new Vector3(u.transform.position.x, u.transform.position.y, -5f);
            var unitClass = u.GetComponent<Unit>();
            unitClass.arPriklausoZaidejui = true;
            player.rankojeUnit = null;
            player.arPestininkasRankoje = false;
            arTusciasLangelis = false;


        }
        if (player.unit != null && player.unit.arGaliPulti == true)
        {
            // gameMaster.paryskintasLangelis.SetActive(true);
            // gameMaster.paryskintasLangelis.transform.position = this.transform.position;

        }
        else
        {
            //gameMaster.paryskintasLangelis.SetActive(false);
        }
        PaspaustaEiti();
    }
    private void OnMouseOver()
    {
        if (player.unit == null)
        {
            sprite.color = gameMaster.langelioSpalvaUzvedusPele;
        }
        else if (GalimasEjimas())
        {
            sprite.color = gameMaster.kariuomenesEjimoSpalvaUzvedusPele;
        }
        if(arAntLangelioEsantiPriesaGalimaPulti)
        {
            sprite.color = gameMaster.kariuomenesPuolimoSpalva;
        }
        if (player.arPestininkasRankoje && arTusciasLangelis)
        {
            sprite.color = gameMaster.kariuomenesEjimoSpalva;
        }
       

    }
    private void OnMouseExit()
    {
        if (player.unit == null)
        {
            sprite.color = dabartineSpalva;
        }
        else if (GalimasEjimas())
        {
            sprite.color = gameMaster.kariuomenesEjimoSpalva;
        }
        if (arAntLangelioEsantiPriesaGalimaPulti)
        {
            sprite.color = gameMaster.kariuomenesPuolimoSpalva;
        }
    }
    private bool GalimasEjimas()
    {

        if (player.unit != null && Mathf.Abs(player.unit.transform.position.x - this.transform.position.x) + Mathf.Abs(player.unit.transform.position.y - this.transform.position.y) <= player.unit.galimasVaiksciotiAtstumas)
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

        if (GalimasEjimas() && player.unit != null && player.unit.arGalimaJudinti == true && arTusciasLangelis)
        {
            StartCoroutine(PradetiJudejima());
            

        }
    }
    IEnumerator PradetiJudejima()
    {
        while (player.unit.transform.position.x != transform.position.x)
        {
            player.unit.transform.position = Vector2.MoveTowards(player.unit.transform.position, new Vector2(transform.position.x, player.unit.transform.position.y), player.unit.judejimoGreitis * Time.deltaTime);
            yield return null;
        }
        while (player.unit.transform.position.y != transform.position.y)
        {
            player.unit.transform.position = Vector2.MoveTowards(player.unit.transform.position, new Vector2(player.unit.transform.position.x, transform.position.y), player.unit.judejimoGreitis * Time.deltaTime);
            yield return null;
        }
        player.unit.arBaigeJudeti = true;
        player.unit.transform.position = new Vector3(player.unit.transform.position.x, player.unit.transform.position.y, -5f);
        player.unit.arGalimaJudinti = false;
        player.unit.arGaliPulti = true;
        player.unit.GalimiPuolimoLangeliai();
        gameMaster.IsvalytiPasirinktusLangelius();
        
        if (player.dabartinisLangelis != null)
        {
            player.dabartinisLangelis.arTusciasLangelis = true;
            player.dabartinisLangelis = this;
        }
        
    }
    
}
