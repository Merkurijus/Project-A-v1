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

        if (player.arKarysRankoje == false)
        {
            pirktiKariUI.SetActive(true);
        }
        else
        {
            pirktiKariUI.SetActive(false);
        }
    }
    // Bendras kariu pirkimo/padejimo metodas
    void PirktiKari(int kaina, GameObject karys)
    {
        
        if (gameMaster.arZaidejoEjimas())
        {
            if (kaina <= player.auksiniai && player.arKarysRankoje == false)
            {
                player.zaidejoKariuomene.Add(karys);
                player.auksiniai -= kaina;
                player.arKarysRankoje = true;
                pirktiKariUI.SetActive(false);
                PadetiKari(karys);
                gameMaster.AtnaujintiAuksiniuTeksta();
            }
            else
            {
                Debug.Log("neuztenka auksiniu");
            }
        }
    }
    void PadetiKari(GameObject karys)
    {
        if (player.rankojeUnit == null && gameMaster.arZaidejoEjimas())
        {
            player.rankojeUnit = karys.GetComponent<Unit>();
            player.unit = null;
            gameMaster.IsvalytiPasirinktusLangelius();
        }
    }
    
    // Kuriami nauji kariai
    // Pestininkai
    public void pirktiPestininka()
    {
        PirktiKari(kainos.PestininkuKaina, gameMaster.pestininkas);
        
    }
    public void PadetiPestininka()
    {
        PadetiKari(gameMaster.pestininkas);

    }
   
}
