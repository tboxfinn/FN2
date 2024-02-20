using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _chr_CamShake : MonoBehaviour
{
    public bool ShakeItOff;

    void Update()
    {
        if(ShakeItOff == true){
            Vector3 originalPos = transform.localPosition;
            float x = Random.Range(-1, 1) * _chr_GUIManager.instance.MagnitudCamShake;
            float y = Random.Range(-1, 1) * _chr_GUIManager.instance.MagnitudCamShake;
            float z = Random.Range(-1, 1) * _chr_GUIManager.instance.MagnitudCamShake;
            transform.localPosition =  new Vector3(x, y, z);
        }
        else if(ShakeItOff == false){
            Vector3 originalPos = transform.localPosition;
            transform.localPosition = originalPos;
        }
    }
}
