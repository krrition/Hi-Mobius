using UnityEngine;

public class NPCWalkState : NPCBaseState
{
    private float elapsedTimer;
    
    public override void EnterState(NPCController npc)
    {
        //start pathing
        npc.navAgent.speed = npc.speed;
        npc.navAgent.angularSpeed = npc.angularSpeed;
    }
    
    public override void UpdateState(NPCController npc)
    {
        elapsedTimer += Time.deltaTime;
        //end of walk time go to idle
        if (elapsedTimer >= npc.walkTime)
        {
            npc.SetState(npc.idleState);
            elapsedTimer = 0;
            return;
        }
        
        //reroute when reached destination
        if (npc.navAgent.remainingDistance <=0)
            npc.Reroute();
    }
    
    
    
}
