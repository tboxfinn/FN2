using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<Entity_Type>
{

    Entity_Type owner;
    
    State<Entity_Type> currentState;

    State<Entity_Type> previousState;

    State<Entity_Type> globalState;

    public StateMachine(Entity_Type _owner)
    {
        owner = _owner;
        currentState = null;
        previousState = null;
        globalState = null;

    }

    public void SetCurrentState(State<Entity_Type> s)
    {
        if(s == null)
        {
            Debug.Log("<StateMachine::ChangeState>: trying to change to a null state");
        }

        if(previousState != null)
        {
            previousState = currentState;
            currentState.Exit(owner);
            
        }

        currentState = s;

        currentState.Enter(owner);
    }

    public void SetGlobalState(State<Entity_Type> s) 
    { 
        globalState = s; 
    }

    public void SetPreviousState(State<Entity_Type> s) 
    { 
        previousState = s; 
    }

    public void Updating()
    {
                
        if (globalState)
        {
            globalState.Excute(owner);
        }
            
        if (currentState)
        {
            currentState.Excute(owner);
        }
    }

    public void ChangeState(State<Entity_Type> pNewState)
    {
        if (pNewState == null)
        {
            Debug.Log("<StateMachine::ChangeState>: trying to change to a null state");
        }

        if (previousState != null)
        {
            previousState = currentState;

        }

        currentState.Exit(owner);     
        currentState = pNewState;

        currentState.Enter(owner);
    }

    public void RevertToPreviousState()
    {
        ChangeState(previousState);
    }

    State<Entity_Type> CurrentState()
    {
        return currentState;
    }

    State<Entity_Type> GlobalState()
    {
        return globalState;
    }

    State<Entity_Type> PreviousState()
    {
        return previousState;
    }

    //Se encuentra en un estado en particular
    public bool IsInstate(State<Entity_Type> st)
    {
        if (st == CurrentState()) return true;
        else return false;
    }
}
 