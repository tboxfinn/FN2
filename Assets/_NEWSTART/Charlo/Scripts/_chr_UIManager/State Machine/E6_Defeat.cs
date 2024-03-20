using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E6_Defeat : State<_chr_GUIManager>
{
    public static E6_Defeat instance = null;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
            
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public override void Enter(_chr_GUIManager entity)
    {
        //Activa los paneles que se utilizarán y desactiva los que ya no se van a utilizar
    }

    public override void Excute(_chr_GUIManager entity)
    {

    }

    public override void Exit(_chr_GUIManager entity)
    {

    }
}
