using Sandbox;

public sealed partial class NPCController : Actor
{
    public NavMeshAgent navAgent;
    public Navigation nav;

    protected override void OnStart()
    {
        base.OnStart();

        navAgent = GameObject.GetComponent<NavMeshAgent>();
        nav = GameObject.AddComponent<Navigation>();

        if ( navAgent == null )
        {
            Log.Error($"NPC {name} has NO NavMeshAgent on GameObject!");
            return;
        }

        Log.Info($"{this} {name} NavMeshAgent OK");
    }

    protected override void OnUpdate()
    {
        UpdateMovement();
        UpdateView();
        base.UpdateStamina();
        base.UpdateStealth();
    }
}
