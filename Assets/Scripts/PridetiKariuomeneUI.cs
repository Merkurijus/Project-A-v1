using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PridetiKariuomeneUI : MonoBehaviour
{
    private Player player;
    private Kainos kainos;
    private GameMaster gameMaster;


    public GameObject pirktiKariUI;
    private void Start()
    {
        kainos = FindObjectOfType<Kainos>();
        gameMaster = FindObjectOfType<GameMaster>();
        player = FindObjectOfType<Player>();
    }
    private void Update()
    {

        if (player.arPestininkasRankoje == false)
        {
            pirktiKariUI.SetActive(true);
        }
        else
        {
            pirktiKariUI.SetActive(false);
        }
    }
    public void pirktiPestininka()
    {
        if (gameMaster.arZaidejoEjimas())
        {
            if (kainos.PestininkuKaina <= player.auksiniai && player.arPestininkasRankoje == false)
            {
                player.zaidejoKariuomene.Add(gameMaster.pestininkas);
                player.auksiniai -= kainos.PestininkuKaina;
                player.arPestininkasRankoje = true;
                pirktiKariUI.SetActive(false);
                PadetiPestininka();
                gameMaster.AtnaujintiAuksiniuTeksta();
            }
            else
            {
                Debug.Log("neuztenka auksiniu");
            }
        }
        
    }
    public void PadetiPestininka()
    {
        if (player.rankojeUnit == null & gameMaster.arZaidejoEjimas())
        {
            player.rankojeUnit = gameMaster.pestininkas.GetComponent<Unit>();
        }

    }
}
