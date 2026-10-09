using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearStateMachine : StateMachine
{

    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public Target Target { get; private set; }

    [field: SerializeField] public float Speed { get; private set; }
    private void Start()
    {
        // Ngăn NavMeshAgent tự động dịch chuyển và xoay nhân vật
        Agent.updatePosition = false;
        Agent.updateRotation = false;
        SwitchState(new BearIdleState(this));
    }
}
