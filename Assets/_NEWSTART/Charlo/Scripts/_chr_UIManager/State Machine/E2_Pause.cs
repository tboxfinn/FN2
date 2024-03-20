using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_Pause : State<_chr_GUIManager>
{
    public static E2_Pause instance = null;

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
        //entity.ExtensionPausePanel.SetActive(true);
        entity.PauseGamePanel.SetActive(true);

        //Aqui aparece el cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public override void Excute(_chr_GUIManager entity)
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            entity.Estados.ChangeState(E3_Game.instance);
        }
    }

    public override void Exit(_chr_GUIManager entity)
    {

    }
}
