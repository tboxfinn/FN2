using System.IO.Pipes;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
using System.Diagnostics;
public class _chr_FreeCam : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float fastMovementSpeed = 100f;
    public float TimerCambio;
    public GameObject Telon;
 
    public TypeCam typeCam;

    void Start()
    {
        CambioCamara(TypeCam.Cam1);
    }

    void Update()
    {
        switch(typeCam){
            case TypeCam.Cam1:
                transform.position = transform.position + (transform.forward * movementSpeed * Time.deltaTime);
            break;

            case TypeCam.Cam2:
                transform.position = transform.position + (transform.right * movementSpeed * Time.deltaTime);
            break;

            case TypeCam.Cam3:
                transform.position = transform.position + (transform.forward * movementSpeed * Time.deltaTime);
            break;

            case TypeCam.Cam4:
                transform.position = transform.position + (transform.forward * movementSpeed * Time.deltaTime);
            break;
        }

        TimerCambio -= 1 * Time.deltaTime;

        if(TimerCambio <= 0){
            int Camaraaleatoria = UnityEngine.Random.Range(1, 5);
            switch(Camaraaleatoria){
                case 1:
                    CambioCamara(TypeCam.Cam1);
                break;

                case 2:
                    CambioCamara(TypeCam.Cam2);
                break;

                case 3:
                    CambioCamara(TypeCam.Cam3);
                break;

                case 4:
                    CambioCamara(TypeCam.Cam4);
                break;
            }
            TimerCambio = UnityEngine.Random.Range(5, 7);
        }
        

    }

    public void CambioCamara(TypeCam camara){
        switch(camara){
            case TypeCam.Cam1:
                typeCam = TypeCam.Cam1;
                transform.position = new Vector3(-231, 5.2f, 43.3f);
                transform.rotation = Quaternion.Euler(0, 90.399f, 0);
            break;

            case TypeCam.Cam2:
                typeCam = TypeCam.Cam2;
                transform.position = new Vector3(-85.3f, 5.2f, 150);
                transform.rotation = Quaternion.Euler(0, 155.337f, 0);
            break;

            case TypeCam.Cam3:
                typeCam = TypeCam.Cam3;
                transform.position = new Vector3(-52.8f, 15.8f, 139);
                transform.rotation = Quaternion.Euler(0, 190.8f, 0);
            break;

            case TypeCam.Cam4:
                typeCam = TypeCam.Cam4;
                transform.position = new Vector3(-223, 5.8f, 0.1f);
                transform.rotation = Quaternion.Euler(0, 115.05f, 0);
            break;

        }
    }

}

public enum TypeCam{
    Cam1, Cam2, Cam3, Cam4
}