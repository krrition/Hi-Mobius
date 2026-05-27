using UnityEngine;

public class NPCIdleState : NPCBaseState
{
    private float elapsedTimer;
    
    public override void EnterState(NPCController npc)
    {
        //stop pathing
        npc.navAgent.speed = 0;
        npc.navAgent.angularSpeed = 0;
    }
    
    public override void UpdateState(NPCController npc)
    {
        elapsedTimer += Time.deltaTime;
        //end of idle time go to walk
        if (elapsedTimer >= npc.idleTime)
        {
            //roll chance to change destination
            if (Random.Range(0, 101) < npc.rerouteChance) 
                npc.Reroute();
            
            npc.SetState(npc.walkState);
            elapsedTimer = 0;
            return;
        }
    }
}
