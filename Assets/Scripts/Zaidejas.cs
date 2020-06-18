using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Zaidejas 
{
    public string vardas;
    public string slaptazodis;
    public int lygis;
    public int taskai;
    public string sesija;
    public Zaidejas(string vardas, string slaptazodis, int lygis, int taskai, string sesija)
    {
        this.vardas = vardas;
        this.slaptazodis = slaptazodis;
        this.lygis = lygis;
        this.taskai = taskai;
        this.sesija = sesija;

    }
    public int LygioPridejimas()
    {
        return this.lygis++;
    }
    public int TaskuPridejimas(int kiek)
    {
        return this.taskai += kiek;
    }
}

[Serializable]
public class Zaidejai
{
    public Zaidejas[] zaidejai;
}
