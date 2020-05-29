using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PridetiKariuomeneUI : MonoBehaviour
{
    private Player player;
    private Kainos kainos;
    private GameMaster gameMaster;
    private void Start()
    {
        kainos = FindObjectOfType<Kainos>();
        gameMaster = FindObjectOfType<GameMaster>();
        player = FindObjectOfType<Player>();
    }
    public void pirktiPestininka()
    {
        if (kainos.PestininkuKaina <= player.auksiniai)
        {
            player.zaidejoKariuomene.Add(gameMaster.pestininkas);
            player.auksiniai -= kainos.PestininkuKaina;
        }
        else
        {
            Debug.Log("neuztenka auksiniu");
        }
    }
    public void PadetiPestininka()
    {
        if (player.rankojeUnit == null)
        {
            player.rankojeUnit = gameMaster.pestininkas.GetComponent<Unit>();
        }
    }
}
