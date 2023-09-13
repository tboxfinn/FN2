using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _tbx_BaseClass : MonoBehaviour
{
    [Header("Keybinds")]
    public KeyCode Hab1;
    public KeyCode Hab2;
    public KeyCode Hab3;
    public KeyCode ActionKey;


    public float health;
    public float maxHealth;

    public void Start()
    {
        health = maxHealth;
        Hab1 = KeyCode.Alpha1;
        Hab2 = KeyCode.Alpha2;
        Hab3 = KeyCode.Alpha3;
        Debug.Log("Base Class");
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
           health = 0;
        }
    }

    public void Heal(float heal)
    {
        health += heal;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(ActionKey))
        {
            MakeAction();
        }
        if (Input.GetKeyDown(Hab1))
        {
            Habilidad1();
        }
        if (Input.GetKeyDown(Hab2))
        {
            Habilidad2();
        }
        if (Input.GetKeyDown(Hab3))
        {
            Habilidad3();
        }
    }

    public virtual void MakeAction()
    {
        Debug.Log("Accion");
    }

    public virtual void Habilidad1()
    {
        Debug.Log("Habilidad 1");
    }

    public virtual void Habilidad2()
    {
        Debug.Log("Habilidad 2");
    }

    public virtual void Habilidad3()
    {
        Debug.Log("Habilidad 3");
    }

}
