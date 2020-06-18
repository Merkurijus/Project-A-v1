using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PridetiKariuomeneUI : MonoBehaviour
{
    private Player zaidejas;
    private Kainos kainos;
    private GameMaster gameMaster;

    [Header("Kariuomenes UI")]
    public GameObject pirktiPestininkaUI;
    public GameObject pirktiLankininkaUI;
    public GameObject pirktiMagaUI;
    private void Start()
    {
        kainos = FindObjectOfType<Kainos>();
        gameMaster = FindObjectOfType<GameMaster>();
        zaidejas = GameObject.Find("/Zaidejai/zaidejas").GetComponent<Player>();
        pirktiPestininkaUI.SetActive(true);
        pirktiLankininkaUI.SetActive(true);
        pirktiMagaUI.SetActive(true);
    }

    #region Kariu pirkimas
    void PirktiKari(int kaina, GameObject karys, GameObject karioUI)
    {
        
        if (gameMaster.arZaidejoEjimas())
        {
            if (kaina <= zaidejas.auksiniai && zaidejas.arKarysRankoje == false && !zaidejas.arJauPuole)
            {
                zaidejas.auksiniai -= kaina;
                zaidejas.arKarysRankoje = true;
                PadetiKari(karys, karioUI);
                zaidejas.rankoje = karys;
                gameMaster.AtnaujintiAuksiniuTeksta();
                gameMaster.IsvalytiPasirinktusLangelius();
                karioUI.SetActive(false);
            }
            else
            {
                Debug.Log("neuztenka auksiniu");
            }
        }
        
    }
    #endregion
    #region Kariu padejimas
    // Kariu padejimas i arena
    void PadetiKari(GameObject karys, GameObject karioUI)
    {
        if (zaidejas.rankoje != null && gameMaster.arZaidejoEjimas() && !zaidejas.arJauPuole)
        {
            
            zaidejas.rankoje = karys;
            gameMaster.IsvalytiPasirinktusLangelius();
            karioUI.SetActive(true);
        }
        
    }
    #endregion
    #region Pestininko pirkimas/padejimas
    // Pestininkai
    public void pirktiPestininka()
    {
        PirktiKari(kainos.PestininkuKaina, gameMaster.kariai[0], pirktiPestininkaUI);
        
    }
    public void PadetiPestininka()
    {
        PadetiKari(gameMaster.kariai[0], pirktiPestininkaUI);

    }
    #endregion
    #region Lankininko pirkimas/padejimas
    public void pirktiLankininka()
    {
        PirktiKari(kainos.LankininkuKaina, gameMaster.kariai[1], pirktiLankininkaUI);

    }
    public void PadetiLankininka()
    {
        PadetiKari(gameMaster.kariai[1], pirktiLankininkaUI);

    }
    #endregion
    #region Magu pirkimas/padejimas
    public void pirktiMaga()
    {
        PirktiKari(kainos.MaguKaina, gameMaster.kariai[2], pirktiMagaUI);

    }
    public void PadetiMaga()
    {
        PadetiKari(gameMaster.kariai[2], pirktiMagaUI);

    }
    #endregion

}
