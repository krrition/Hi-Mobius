using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NPCController : MonoBehaviour
{
    //drag references
    public NavMeshAgent navAgent;
    public Transform target;
    
    //Speed
    public float speed;
    public float angularSpeed;

    //States
    private NPCBaseState currentState;
    public NPCIdleState idleState = new NPCIdleState();
    public NPCWalkState walkState = new NPCWalkState();
    
    //State Variables
    public float idleTime;
    [Range(0, 100)] public int rerouteChance;
    public float walkTime;
    
    
    private void Start()
    {
        //add randomness to variables
        navAgent.speed = Random.Range(2f, 10f);
        idleTime = Random.Range(0.4f, 6f);
        rerouteChance = Random.Range(0, 101);
        walkTime = Random.Range(5f, 15f);
        
        //store speeds
        speed = navAgent.speed;
        angularSpeed = navAgent.angularSpeed;
        
        //set target destination
        navAgent.SetDestination(target.position);
        
        //enter idle state
        currentState = idleState;
        currentState.EnterState(this);
    }
    
    
    private void Update()
    {
        //use current state's update
        currentState.UpdateState(this);
    }

    public void SetState(NPCBaseState state)
    {
        //start new state
        currentState = state;
        currentState.EnterState(this);
    }
    
    public void Reroute()
    {
        //choose what axes of the destination to flip
        int axis = Random.Range(0, 3);
        //temp variable
        var tempPos = target.localPosition;
        switch (axis)
        {
            case 0:
                tempPos.x = -tempPos.x;
                break;
            case 1:
                tempPos.z = -tempPos.z;
                break;
            case 2:
                tempPos.x = -tempPos.x;
                tempPos.z = -tempPos.z;
                break;
        }
        //plug in temp var
        target.localPosition = tempPos;
    
        
        //calculate new destination path
        navAgent.SetDestination(target.position);
    }
}
