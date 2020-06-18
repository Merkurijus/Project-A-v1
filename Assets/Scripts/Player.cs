using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int auksiniai;
    public int lygis;
    public int taskai;
    public List<Unit> PriesaiEsantysNetoli = new List<Unit>();

    private Player player;
    public Unit unit;
    public Tile dabartinisLangelis;
    public Tile naujasLangelis;
    public GameObject rankoje;

    public string vardas;
    public string sesija;
    
    public bool arKarysRankoje;
    public bool arZaidejoEjimas;
    public bool arPuolimoFaze;
    public bool arJauPuole;
    public bool arGalimaJudintiKitaKari;
    private void Start()
    {
         player = GameObject.Find("/Zaidejai/zaidejas").GetComponent<Player>();
         vardas = PlayerPrefs.GetString("vardas", "nera");
         sesija = PlayerPrefs.GetString("sesija", "nera");
         Formos forma = new Formos();
         StartCoroutine(forma.VartotojoInformacija(player, vardas, sesija));
    }
  
   

}
