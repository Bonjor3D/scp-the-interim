using System;
using Sandbox;

public sealed partial class Navigation : Component
{
    public NPCController npc;

	protected override void OnStart()
	{
		npc = GetComponent<NPCController>();
        npc.OnMarkReached += HandleMarkReached;
	}
    private void HandleMarkReached( NPCController npc, Mark mark )
    {
        Log.Info($"{npc.name} reached mark {mark.markId}");

    }
    public void GoToMarkById( string markId, NavMeshAgent navAgent )
	{
		var mark = MarkManager.GetMarkById(markId);

		if ( mark == null )
		{
			Log.Error($"Mark '{markId}' not found");
			return;
		}

		if ( navAgent == null )
		{
			Log.Error("NavMeshAgent is NULL");
			return;
		}

		navAgent.MoveTo( mark.WorldPosition );
	}
}

public class Mark : Component
{

    private readonly List<NPCController> linkedNPCs = new();
    public IReadOnlyList<NPCController> LinkedNPCs => linkedNPCs;
    [Property] public string markId;

    public Actor linkedActor;
    public Vector3 coordinates;

	protected override void OnUpdate()
	{
		if (linkedActor == null) return;
	}
    public void RegisterNPC( NPCController npc )
    {
        if ( npc == null )
            return;

        if ( linkedNPCs.Contains(npc) )
            return;

        linkedNPCs.Add(npc);
        Log.Info($"NPC {npc.name} registered to mark {markId}");
    }
    protected override void OnStart()
    {
        coordinates = WorldPosition;
        MarkManager.Register(this);
    }
    public void UnregisterNPC( NPCController npc )
    {
        if ( npc == null )
            return;

        linkedNPCs.Remove(npc);
        Log.Info($"NPC {npc.name} unregistered from mark {markId}");
    }
}