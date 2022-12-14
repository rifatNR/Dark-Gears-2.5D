using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldChaseState : BlackKnightBaseState
{
    public OldChaseState(BlackKnightStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        Debug.Log("Enter");
    }

    public override void Tick(float delta)
    {
        Debug.Log("Tick");
    }

    public override void Exit()
    {
        Debug.Log("Exit");
    }
}
