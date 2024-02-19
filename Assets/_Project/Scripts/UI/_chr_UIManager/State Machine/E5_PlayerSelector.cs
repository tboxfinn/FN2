using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class E5_PlayerSelector : State<_chr_GUIManager>
{
    public static E5_PlayerSelector instance = null;

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
        SceneManager.LoadScene("_tbx_Characterselect");
        entity.DesactivarPaneles();
        entity.PlayerSelectorPanel.SetActive(true);
    }

    public override void Excute(_chr_GUIManager entity)
    {

    }

    public override void Exit(_chr_GUIManager entity)
    {

    }
}
