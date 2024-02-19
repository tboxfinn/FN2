using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class E3_Game : State<_chr_GUIManager>
{
    public static E3_Game instance = null;
    public _chr_CamShake CamShake;

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

        //Cambia la escena
        SceneManager.LoadScene("_chr_Scene");

        
    }

    public override void Excute(_chr_GUIManager entity)
    {
        for (int i = 0; i < 1 ; i++){
            CamShake = GameObject.Find("CamHolder").GetComponent<_chr_CamShake>();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape)){
            entity.Estados.ChangeState(E2_Pause.instance);
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            CamShake.ShakeItOff = true;
        }
        else if(Input.GetKeyUp(KeyCode.Space)){
            CamShake.ShakeItOff = false;
        }
    }

    public override void Exit(_chr_GUIManager entity)
    {

    }
}
