using Sandbox;

public sealed partial class NPCController : Actor
{
    public NavMeshAgent navAgent;

    [Property] public string npcName;

    protected override void OnStart()
    {
        navAgent = GameObject.GetComponent<NavMeshAgent>();

        if ( navAgent == null )
        {
            Log.Error($"NPC {npcName} has NO NavMeshAgent on GameObject!");
            return;
        }

        Log.Info($"{this} {npcName} NavMeshAgent OK");
    }

    protected override void OnUpdate()
    {
        UpdateMovement();
        UpdateView();
        base.UpdateStamina();
        base.UpdateStealth();
    }
}
