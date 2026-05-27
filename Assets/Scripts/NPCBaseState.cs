using Unity.VisualScripting;
using UnityEngine;

public abstract class NPCBaseState
{
    public abstract void EnterState(NPCController npc);
    
    public abstract void UpdateState(NPCController npc);
}
