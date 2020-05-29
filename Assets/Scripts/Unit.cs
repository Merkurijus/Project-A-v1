using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string pavadinimas;
    public int ataka;
    public int gynybaPriesPestininkus;
    public int gynybaPriesRaitus;
    public string tipas;
    public int gynybaPriesMagija;
    public int galimasVaiksciotiAtstumas;

    // Uzdeti ginklai ir sarvai
    private Ginklai ginklas;
    private Sarvai sarvai;
    private Player player;
    public bool arGalimaJudinti;
    public bool arGaliPulti;
    public bool arPasirinktas;

    

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }
    private void OnMouseDown()
    {
        arPasirinktas = !arPasirinktas;
        if (arPasirinktas)
        {
            player.unit = this;
            Debug.Log("pasirinktas");
            GalimiLangeliai();
        }
        else
        {
            player.unit = null;
            Debug.Log("null");
        }

    }
    private void GalimiLangeliai()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (Mathf.Abs(player.unit.transform.position.x - tile.transform.position.x) + Mathf.Abs(player.unit.transform.position.y - tile.transform.position.y) < galimasVaiksciotiAtstumas)
            {
               // tile.GetComponent<SpriteRenderer>().color = kariuomenesEjimasSpalva;
            }
        }
    }
}
