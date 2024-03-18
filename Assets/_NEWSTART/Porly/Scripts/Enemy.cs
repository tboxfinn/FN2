using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public Quaternion angulo;
    public float grados;
    public int velmov;

    public GameObject target;

    void Start()
    {
        //ani = GetComponent<Animator>();

        target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        comportamientoEnemigo();        
    }

    public void comportamientoEnemigo()
    {
        if(Vector3.Distance(transform.position, target.transform.position) > 5)
        {
            cronometro += 1 * Time.deltaTime;
            if (cronometro >= 4)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }
            switch (rutina)
            {
                case 0:
                    Debug.Log("No camina");
                    break;

                case 1:
                    grados = Random.Range(0, 360);
                    angulo = Quaternion.Euler(0, grados, 0);
                    rutina++;
                    break;

                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angulo, 0.5f);
                    transform.Translate(Vector3.forward * velmov * Time.deltaTime);
                    break;
            }
        }
        else
        {
            var lookPos = target.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 2);

            transform.Translate(Vector3.forward * (velmov + 2) * Time.deltaTime);
        }

    }
}
