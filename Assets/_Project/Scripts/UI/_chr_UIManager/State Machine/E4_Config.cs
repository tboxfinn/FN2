using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E4_Config : State<_chr_GUIManager>
{
    public static E4_Config instance = null;

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
        entity.DesactivarPaneles();
        entity.ConfigPanel.SetActive(true);
    }

    public override void Excute(_chr_GUIManager entity)
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            entity.Estados.ChangeState(E2_Pause.instance);
        }

        //Resolución de pantalla
        switch(entity.resolucion.value){
            //resolución 1920 X 1080X (Recomendar pantalla completa)
            case 0:
                Screen.SetResolution(1920, 1080, entity.IsScreenWindowed);
            break;
            //resolución 1280 X 720 (Recomendado)
            case 1:
                Screen.SetResolution(1280, 720, entity.IsScreenWindowed);
            break;
            //resolución 720 X 405
            case 2:
                Screen.SetResolution(720, 405, entity.IsScreenWindowed);
            break;
        }

        switch(entity.IsScreenWindowed){
            case true:
                entity.WindowedStatus.text = "PANTALLA COMPLETA: OFF";
            break;
            case false:
                entity.WindowedStatus.text = "PANTALLA COMPLETA: ON";
            break;
        }
    }

    public override void Exit(_chr_GUIManager entity)
    {

    }
}
