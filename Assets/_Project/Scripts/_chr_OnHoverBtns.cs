using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class _chr_OnHoverBtns : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] bool IsOnHover;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(IsOnHover){
            case true:
            transform.localScale = new Vector3(1.5f, 1.5f, 0);
            break;

            case false:
            transform.localScale = new Vector3(1, 1, 0);
            break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsOnHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsOnHover = false;
    }
}
