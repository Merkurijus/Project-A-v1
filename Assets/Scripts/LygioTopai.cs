using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LygioTopai
{
    public string vardas;
    public int lygis;
    public LygioTopai(string vardas, int lygis)
    {
        this.vardas = vardas;
        this.lygis = lygis;
    }
}
[Serializable]
public class LygiuTopuMasyvas
{
    public LygioTopai[] lygioTopuMasyvas;
}
