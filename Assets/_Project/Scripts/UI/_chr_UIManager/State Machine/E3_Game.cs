using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E3_Game : State<_chr_GUIManager>
{
    public static E3_Game instance = null;

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

        // Aqui vuelve a desaparecer el cursor
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
    }

    public override void Excute(_chr_GUIManager entity)
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            entity.Estados.ChangeState(E2_Pause.instance);
        }
    }

    public override void Exit(_chr_GUIManager entity)
    {

    }
}
