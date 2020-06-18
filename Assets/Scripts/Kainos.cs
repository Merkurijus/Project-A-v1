using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kainos : MonoBehaviour
{

    public int PestininkuKaina { get; private set; }
    public int LankininkuKaina { get; private set; }
    public int MaguKaina { get; private set; }


    private void Start()
    {
        PestininkuKaina = 20;
        LankininkuKaina = 40;
        MaguKaina = 60;
    }

}
