using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PridetiKariuomeneUI : MonoBehaviour
{
    private Player zaidejas;
    private Kainos kainos;
    private GameMaster gameMaster;


    public GameObject pirktiKariUI;
    private void Start()
    {
        kainos = FindObjectOfType<Kainos>();
        gameMaster = FindObjectOfType<GameMaster>();
        zaidejas = GameObject.Find("/Zaidejai/zaidejas").GetComponent<Player>();
        pirktiKariUI.SetActive(true);
    }
  
    // Bendras kariu pirkimo/padejimo metodas
    void PirktiKari(int kaina, GameObject karys)
    {
        
        if (gameMaster.arZaidejoEjimas())
        {
            if (kaina <= zaidejas.auksiniai && zaidejas.arKarysRankoje == false && !zaidejas.arJauPuole)
            {
                zaidejas.zaidejoKariuomene.Add(karys);
                zaidejas.auksiniai -= kaina;
                zaidejas.arKarysRankoje = true;
                PadetiKari(karys);
                zaidejas.rankojeUnit = karys.GetComponent<Unit>();
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
        if (zaidejas.rankojeUnit != null && gameMaster.arZaidejoEjimas() && !zaidejas.arJauPuole)
        {
            zaidejas.rankojeUnit = karys.GetComponent<Unit>();
            zaidejas.unit = karys.GetComponent<Unit>();
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
