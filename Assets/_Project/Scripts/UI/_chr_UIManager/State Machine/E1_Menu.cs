using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class E1_Menu : State<_chr_GUIManager>
{
    public static E1_Menu instance = null;

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
        entity.MainMenuPanel.SetActive(true);
        SceneManager.LoadScene("_chr_MenuScene");
    }

    public override void Excute(_chr_GUIManager entity)
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            entity.Estados.ChangeState(E5_PlayerSelector.instance);
        }
    }

    public override void Exit(_chr_GUIManager entity)
    {

    }
}
