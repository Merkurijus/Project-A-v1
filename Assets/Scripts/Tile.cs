using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private GameMaster gameMaster;
    private Player player;

    private SpriteRenderer sprite;
    private Color dabartineSpalva;
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
       

        if (player.unit != null && player.unit.arGaliPulti == true)
        {
            gameMaster.paryskintasLangelis.SetActive(true);
            gameMaster.paryskintasLangelis.transform.position = this.transform.position;
        }
        else
        {
            gameMaster.paryskintasLangelis.SetActive(false);
        }
    }
    private void OnMouseOver()
    {
        sprite.color = gameMaster.langelioSpalvaUzvedusPele;
    }
    private void OnMouseExit()
    {
        sprite.color = dabartineSpalva;
    }
}
