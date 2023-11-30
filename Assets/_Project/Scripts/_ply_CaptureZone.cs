using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class _ply_CaptureZone : MonoBehaviour
{
    public Collider zonaCollider;
    public float tiempoCaptura = 5f;
    public float progresoCaptura = 0f;
    public bool zonaCapturada;
    public int cantGente;
    private void Start()
    {
        zonaCapturada = false;
        cantGente = 0;
    }
    private void Update()
    {
        if (zonaCapturada)
        {
            return;
        }
        if (cantGente == 1)
        {
            progresoCaptura += Time.deltaTime;
            if (progresoCaptura >= tiempoCaptura)
            {
                CapturarZona();
            }
        }
        else
        {
            //progresoCaptura = Mathf.Max(0f, progresoCaptura - Time.deltaTime);
            Debug.Log("Hay m�s de una persona en la zona");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (zonaCapturada)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            cantGente++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (zonaCapturada)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            cantGente--;
        }
    }
    private void CapturarZona()
    {
        progresoCaptura = 0f;
        //jugadorCapturando = null;
        Debug.Log("Zona capturada completamente");
        zonaCapturada = true;
    }
}