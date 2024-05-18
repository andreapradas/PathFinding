using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class PanelMonedas : MonoBehaviour
{
    public TMP_Text textoMonedas;
    private int monedasRecogidas;

    void Start()
    {
        ActualizarTexto(0);
    }

    public void ActualizarTexto(int monedasRecogidas)
    {
        textoMonedas.text = "Monedas recogidas: " + monedasRecogidas.ToString();
        
    }
    public void IncrementarMonedas()
    {
        monedasRecogidas++;
        ActualizarTexto(monedasRecogidas);
        //Mostrar progreso
        Debug.Log("¡Enhorabuena, llevas "+ monedasRecogidas + " monedas recogidas!"); 
         //Compruebo si se ha acabado el juego
        if(monedasRecogidas == 12)
        {
            //Cambiar de escena, juego finalizado
            SceneManager.LoadScene("EndGame");
        }
    }
}
