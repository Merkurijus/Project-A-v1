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
        pirktiKariUI.SetActive(true);
    }
  
    // Bendras kariu pirkimo/padejimo metodas
    void PirktiKari(int kaina, GameObject karys)
    {
        
        if (gameMaster.arZaidejoEjimas())
        {
            if (kaina <= player.auksiniai && player.arKarysRankoje == false && !player.arJauPuole)
            {
                player.zaidejoKariuomene.Add(karys);
                player.auksiniai -= kaina;
                player.arKarysRankoje = true;
                PadetiKari(karys);
                player.rankojeUnit = karys.GetComponent<Unit>();
                gameMaster.AtnaujintiAuksiniuTeksta();
                gameMaster.IsvalytiPasirinktusLangelius();
                pirktiKariUI.SetActive(false);
            }
            else
            {
                Debug.Log("neuztenka auksiniu");
            }
        }
        
    }
    void PadetiKari(GameObject karys)
    {
        if (player.rankojeUnit != null && gameMaster.arZaidejoEjimas() && !player.arJauPuole)
        {
            player.rankojeUnit = karys.GetComponent<Unit>();
            player.unit = karys.GetComponent<Unit>();
            gameMaster.IsvalytiPasirinktusLangelius();
            pirktiKariUI.SetActive(true);
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
