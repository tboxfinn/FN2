using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<Entity_Type> : MonoBehaviour
{
    

    public virtual void Enter(Entity_Type entity)
    {

    }

    public virtual void Excute(Entity_Type entity)
    {

    }

    public virtual void Exit( Entity_Type entity)
    {

    }
}
