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
    
    //Speeds
    public float speed;
    [HideInInspector] public float angularSpeed;
    
    //States
    private NPCBaseState currentState;
    [HideInInspector] public NPCIdleState idleState = new NPCIdleState();
    [HideInInspector] public NPCWalkState walkState = new NPCWalkState();
    
    [Header("State Variables (for debugging)")]
    public float idleTime;
    [Range(0, 100)] public int rerouteChance;
    public float walkTime;

    [Header("Variable Min/Max")] 
    public float speedMin = 2f;
    public float speedMax = 6f;
    public float idleMin = 0.4f;
    public float idleMax = 6f;
    public float walkMin = 5f;
    public float walkMax = 15f;
    
    private void Start()
    {
        //add randomness to variables using min max
        navAgent.speed = Random.Range(speedMin, speedMax);
        idleTime = Random.Range(idleMin, idleMax);
        rerouteChance = Random.Range(0, 101);
        walkTime = Random.Range(walkMin, walkMax);
        
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
