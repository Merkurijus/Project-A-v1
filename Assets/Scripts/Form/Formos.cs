using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Text;

public class Formos : MonoBehaviour
{
    string registracijosNuoroda = "http://185.80.129.145/register.php";
    string prisijingumoNuoroda = "http://185.80.129.145/login.php";
    string vartotojoInformacija = "http://185.80.129.145/vartotojas.php";
    string lygioTopai = "http://185.80.129.145/lygioTopai.php";
    [Header("Registracijos forma")]
    public TMP_InputField vardasInput;
    public TMP_InputField slaptazodisInput;
    public TMP_InputField slaptazodis2Input;
    public TMP_Text pranesimas;
    [Header("Prisijungimo forma")]
    public TMP_InputField vardasPrisijungimasInput;
    public TMP_InputField slaptazodisPrisijungimasInput;
    public TMP_Text pranesimasPrisijungimas;
    [Header("Spalvos")]
    public Color klaidosSpalva;
    public Color sekmesSpalva;
    [Header("Formos")]
    public GameObject prisijungimoForma;
    public GameObject registracijosForma;
   

    private void Start()
    {
       
        pranesimas.text = "";
    }
    public void RegistracijosMygtukas()
    {
        StartCoroutine(Registracija(vardasInput.text, slaptazodisInput.text, slaptazodis2Input.text, SHA512(vardasInput.text + slaptazodisInput.text)));
        
    }
    public void PrisijungimoMygtukas()
    {
        StartCoroutine(Prisijungimas(vardasPrisijungimasInput.text, slaptazodisPrisijungimasInput.text, SHA512(vardasInput.text + slaptazodisInput.text)));
        
    }
    public void AtidarytiRegistracijosForma()
    {
        pranesimas.text = "";
        prisijungimoForma.SetActive(false);
        registracijosForma.SetActive(true);
    }
    public void AtidarytiPrisijungimoForma()
    {
        pranesimas.text = "";
        registracijosForma.SetActive(false);
        prisijungimoForma.SetActive(true);
    }
    IEnumerator Registracija(string vardas, string slaptazodis, string slaptazodis2, string sesija)
    {
        WWWForm form = new WWWForm();
        form.AddField("vardas", vardas);
        form.AddField("slaptazodis", slaptazodis);
        form.AddField("slaptazodis2", slaptazodis2);
        form.AddField("sesija", sesija);

        using (UnityWebRequest www = UnityWebRequest.Post(registracijosNuoroda, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                pranesimas.text = www.downloadHandler.text;
                if (www.downloadHandler.text.Equals("Sekmingai uzsiregistravote!!!"))
                {
                    sesija = SHA512(vardas + slaptazodis);
                    PlayerPrefs.SetString("vardas", vardas);
                    PlayerPrefs.SetString("sesija", sesija);
                    PlayerPrefs.Save();
                    pranesimas.color = sekmesSpalva;
                    yield return new WaitForSeconds(2f);
                    SceneManager.LoadScene(1);
                    
                }
                else
                {
                    pranesimas.color = klaidosSpalva;
                }
                
                
            }
        }
    }
    IEnumerator Prisijungimas(string vardas, string slaptazodis, string sesija)
    {
        WWWForm form = new WWWForm();
        form.AddField("vardas", vardas);
        form.AddField("slaptazodis", slaptazodis);
        form.AddField("sesija", sesija);
        using (UnityWebRequest www = UnityWebRequest.Post(prisijingumoNuoroda, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                pranesimasPrisijungimas.text = www.downloadHandler.text;
                if (www.downloadHandler.text.Equals("Sekmingai prisijungete!!!"))
                {
                    pranesimasPrisijungimas.color = sekmesSpalva;

                    PlayerPrefs.SetString("vardas", vardas);
                    PlayerPrefs.SetString("sesija", sesija);
                    PlayerPrefs.Save();
                  
                    yield return new WaitForSeconds(2f);
                    SceneManager.LoadScene(1);
                }
                else
                {
                    pranesimasPrisijungimas.color = klaidosSpalva;
                }


            }
        }
    }
    public static string SHA512(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        using (var hash = System.Security.Cryptography.SHA512.Create())
        {
            var hashedInputBytes = hash.ComputeHash(bytes);

            // Convert to text
            // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
            var hashedInputStringBuilder = new StringBuilder(128);
            foreach (var b in hashedInputBytes)
                hashedInputStringBuilder.Append(b.ToString("X2"));
            return hashedInputStringBuilder.ToString();
        }
    }
    public IEnumerator VartotojoInformacija(Player player, string vardas, string sesija)
    {
        WWWForm form = new WWWForm();
        form.AddField("vardas", vardas);
        form.AddField("sesija", sesija);

        using (UnityWebRequest www = UnityWebRequest.Post(vartotojoInformacija, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
               
            }
            else
            {
                string uzklausa = www.downloadHandler.text;
                uzklausa = uzklausa.Trim('[', ']');
                Zaidejas zaidejas = JsonUtility.FromJson<Zaidejas>(uzklausa);
                player.taskai = zaidejas.taskai;
                player.lygis = zaidejas.lygis;
            }
        }
    }
    public IEnumerator LygioTopai(string vardas)
    {
        WWWForm form = new WWWForm();
        form.AddField("vardas", vardas);
    

        using (UnityWebRequest www = UnityWebRequest.Post(lygioTopai, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);

            }
            else
            {
                string uzklausa = www.downloadHandler.text;
                uzklausa = uzklausa.Trim('[', ']');
                Debug.Log(uzklausa);
                LygiuTopuMasyvas topai = JsonUtility.FromJson<LygiuTopuMasyvas>(uzklausa);
                //Debug.Log(topai.lygioTopuMasyvas[0].vardas);
            }
        }
    }
}
