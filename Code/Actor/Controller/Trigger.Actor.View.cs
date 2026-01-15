using Sandbox;
using System;

public partial class Actor
{
    public event Action<Actor> OnShadowEntered;
    public event Action<Actor> OnShadowExited;
    public void EnterShadow()
    {
        if ( IsInShadow )
            return;

        IsInShadow = true;
        OnShadowEntered?.Invoke( this );
    }

    public void ExitShadow()
    {
        if ( !IsInShadow )
            return;

        IsInShadow = false;
        OnShadowExited?.Invoke( this );
    }
}

public sealed class ShadowTrigger : Component
{
    private Collider collider;

    [Property] string id;
    [Property] bool IsEnabled;

    protected override void OnStart()
    {
        collider = GetComponent<Collider>();

        if ( collider == null )
        {
            Log.Error("ShadowTrigger: Collider not found");
            return;
        }

        collider.OnTriggerEnter += HandleEnter;
        collider.OnTriggerExit  += HandleExit;
    }

    private void HandleEnter( Collider other )
    {
        if(!IsEnabled) return;
        
        var actor = other.GetComponent<Actor>();
        if ( actor == null )
            return;

        actor.EnterShadow();
        Log.Info($"{actor.GameObject.Name} entered shadow");
    }

    private void HandleExit( Collider other )
    {
        if(!IsEnabled) return;

        var actor = other.GetComponent<Actor>();
        if ( actor == null )
            return;

        actor.ExitShadow();
        Log.Info($"{actor.GameObject.Name} exited shadow");
    }
}
