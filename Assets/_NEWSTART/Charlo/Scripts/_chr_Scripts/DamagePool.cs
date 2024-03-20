using System.Collections.Generic;
using UnityEngine;

public class DamagePool : MonoBehaviour
{
    [SerializeField] private GameObject DamagePrefab;
    [SerializeField] private List<GameObject> DamageList;
    [SerializeField]private int poolSize; 

    public static DamagePool instance;

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

    // Start is called before the first frame update
    void Start()
    {
        AddDamageToPool(poolSize);
    }

    private void AddDamageToPool(int amount){
        for (int i = 0; i < amount; i++){
            GameObject damage = Instantiate(DamagePrefab);
            damage.SetActive(false);
            DamageList.Add(damage);
            damage.transform.parent = transform;
        }
    }

    public GameObject RequestDamage(){
        for (int i = 0; i < DamageList.Count; i++){
            if(!DamageList[i].activeSelf){
                DamageList[i].SetActive(true);
                DamageList[i].transform.position = this.transform.position;
                
                return DamageList[i];
            }
        }

        return null;
    }
}
