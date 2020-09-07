using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : IState
{


    public Idle()  
    {
        
    }

    public void OnEnter()
    {

    }

    public void Tick()
    {
        Debug.Log("im idle");
    }

    public void FixedTick()
    {

    }

    public void OnExit()
    {
        
    }
}
