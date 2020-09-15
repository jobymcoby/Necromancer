using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchForEnemy : IState
{
    private float _detectionRange;

    public SearchForEnemy(float detectionRange)  
    {
        _detectionRange = detectionRange;
    }

    public void OnEnter()
    {

    }

    public void Tick()
    {
        // play Idle
    }

    public void FixedTick()
    {

    }

    public void OnExit()
    {
        
    }
}
