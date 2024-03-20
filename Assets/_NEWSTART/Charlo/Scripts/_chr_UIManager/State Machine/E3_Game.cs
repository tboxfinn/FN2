using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        entity.DesactivarPaneles();
        entity.InGamePanel.SetActive(true);

        // Aqui vuelve a desaparecer el cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //Busca el componente _chr_CamShake
        entity.CamShake = GameObject.Find("Main Camera").GetComponent<_chr_CamShake>();

        
    }

    public override void Excute(_chr_GUIManager entity)
    {

    }

    public override void Exit(_chr_GUIManager entity)
    {

    }
}
