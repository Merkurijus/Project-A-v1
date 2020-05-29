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
    
    private void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        player = FindObjectOfType<Player>();
        sprite = GetComponent<SpriteRenderer>();
        dabartineSpalva = sprite.color;
    }
    private void OnMouseDown()
    {
       
        if (player.rankojeUnit != null)
        {
            var u = Instantiate(gameMaster.pestininkas);
            u.transform.position = this.transform.position;
            player.rankojeUnit = null;
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
    }
    private void OnMouseOver()
    {
        if (player.unit == null)
        {
            sprite.color = gameMaster.langelioSpalvaUzvedusPele;
        }
        else if (Mathf.Abs(player.unit.transform.position.x - transform.position.x) + Mathf.Abs(player.unit.transform.position.y - transform.position.y) <= player.unit.galimasVaiksciotiAtstumas)
        {
            sprite.color = gameMaster.kariuomenesEjimoSpalvaUzvedusPele;
        }

    }
    private void OnMouseExit()
    {
        if (player.unit == null)
        {
            sprite.color = dabartineSpalva;
        }
        else if (Mathf.Abs(player.unit.transform.position.x - transform.position.x) + Mathf.Abs(player.unit.transform.position.y - transform.position.y) <= player.unit.galimasVaiksciotiAtstumas)
        {
            sprite.color = gameMaster.kariuomenesEjimoSpalva;
        }
    }
   
}
