using System;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Transform target;


    private void Start()
    {
        navAgent.destination = target.position;
    }
}
