using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsCollect : MonoBehaviour
{
    public PanelMonedas panelMonedas;
    private void OnTriggerEnter(Collider other)
    {
        //Compruebo que lo que ha entrado es el player
        if(other.CompareTag("Player"))
        {
            //Desaparecer moneda
            gameObject.SetActive(false);
            //Sumar puntuacion monedas recogidas
            panelMonedas.IncrementarMonedas();
        }
    }
}
