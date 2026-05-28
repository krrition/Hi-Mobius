using Unity.VisualScripting;
using UnityEngine;

public class NPCWalkState : NPCBaseState
{
    private float elapsedTimer;
    
    public override void EnterState(NPCController npc)
    {
        //start pathing
        npc.navAgent.speed = npc.speed;
        npc.navAgent.angularSpeed = npc.angularSpeed;
        
        //switch animation and speed
        npc.animator.SetBool("Walking",true);
        npc.CalibrateSpeed();
    }
    
    public override void UpdateState(NPCController npc)
    {
        elapsedTimer += Time.deltaTime;
        //end of walk time go to idle
        if (elapsedTimer >= npc.walkTime)
        {
            elapsedTimer = 0;
            npc.SetState(npc.idleState);
            return;
        }
        
        //reroute when reached destination
        if (npc.navAgent.remainingDistance <= npc.destinationRadius)
        {
            elapsedTimer = 0;
            npc.Reroute();
            npc.SetState(npc.idleState);
            return;
        }
    }

    



}
