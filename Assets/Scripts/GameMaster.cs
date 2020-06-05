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

    private Player player;
    public float ejimoLaikas = 30f;
    private float dabartinisLaikas;
    private bool arZaidimasPrasidejo;

    private void Start()
    {
        player = FindObjectOfType<Player>();
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
            langelis.GetComponent<SpriteRenderer>().color = langelis.dabartineSpalva;
            if (player.unit != null && langelis.transform.position.x == player.unit.transform.position.x && langelis.transform.position.y == player.unit.transform.position.y)
            {
                langelis.arTusciasLangelis = false;
            }
        }
    }
    public void PradetiEjima()
    {
            int ejimas = Random.Range(0, 1);
            player.arZaidejoEjimas = (ejimas == 1) ? true : false;
            arZaidimasPrasidejo = true;
            dabartinisLaikas = ejimoLaikas;
            AtnaujintiKienoEjimasVarda();

    }
    public void BaigtiEjima()
    {
            player.arZaidejoEjimas = (player.arZaidejoEjimas == true) ? false : true;
            dabartinisLaikas = ejimoLaikas;
            AtnaujintiKienoEjimasVarda();
            AtnaujintiKariuLeidimus(player.arZaidejoEjimas);
            IsvalytiPasirinktusLangelius();
            player.unit = null;
    }
    public void AtnaujintiAuksiniuTeksta()
    {
        auksiniai.text = player.auksiniai.ToString();
    }
    public void AtnaujintiEjimoLaikoTeksta(float dabartinisLaikas)
    {
        laikas.text = Mathf.Round(dabartinisLaikas).ToString();
    }
    public void AtnaujintiKienoEjimasVarda()
    {
        string kienoEjimasText = (player.arZaidejoEjimas) ? "Žaidėjo ėjimas" : "AI ėjimas";
        kienoEjimas.text = kienoEjimasText;
    }
    public bool arZaidejoEjimas()
    {
        if (player.arZaidejoEjimas)
        {
            return true;
        }
        else
        {
            return false;
        }
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
}

