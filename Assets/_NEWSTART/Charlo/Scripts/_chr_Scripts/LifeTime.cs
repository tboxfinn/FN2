using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTime : MonoBehaviour
{
    public float Timer;
    private float TimeRemain;
    // Start is called before the first frame update
    void Start()
    {
        TimeRemain = Timer;
    }

    // Update is called once per frame
    void Update()
    {
        TimeRemain -= Time.deltaTime;

        if(TimeRemain <=  0){
            gameObject.SetActive(false);
            TimeRemain = Timer;
        }
    }
}
