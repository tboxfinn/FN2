using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _chr_CamShake : MonoBehaviour
{
    public bool ShakeItOff;
    public float mag = 0.2f;

    void Update()
    {
        if(ShakeItOff == true){
            Vector3 originalPos = transform.localPosition;
            float x = Random.Range(-1, 1) * mag;
            float y = Random.Range(-1, 1) * mag;
            float z = Random.Range(-1, 1) * mag;
            transform.localPosition =  new Vector3(x, y, z);
        }
        else if(ShakeItOff == false){
            Vector3 originalPos = transform.localPosition;
            transform.localPosition = originalPos;
        }
    }
}
