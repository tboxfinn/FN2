using UnityEngine;

public class ply_CaptureZone : MonoBehaviour
{
    public Collider zonaCollider;
    public float tiempoCaptura = 5f;
    private GameObject jugadorCapturando = null;
    public float progresoCaptura = 0f;
    public bool zonaCapturada;

    private void Start()
    {
        zonaCapturada = false;
    }

    private void Update()
    {
        if (zonaCapturada)
        {
            return;
        }

        if (jugadorCapturando != null)
        {
            progresoCaptura += Time.deltaTime;

            if (progresoCaptura >= tiempoCaptura)
            {
                CapturarZona(jugadorCapturando);
            }
        }
        else
        {
            progresoCaptura = Mathf.Max(0f, progresoCaptura - Time.deltaTime);
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
            if (jugadorCapturando == null)
            {
                jugadorCapturando = other.gameObject;
                Debug.Log("Capturando");
            }
            else if (jugadorCapturando != other.gameObject)
            {
                Debug.Log("Ya hay alguien capturando");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (zonaCapturada)
        {
            return;
        }

        if (other.gameObject == jugadorCapturando)
        {
            jugadorCapturando = null;
        }
    }

    private void CapturarZona(GameObject jugador)
    {
        Debug.Log("Zona capturada completamente por " + jugador.name);
        progresoCaptura = 0f;
        //jugadorCapturando = null;
        zonaCapturada = true;
    }
}
