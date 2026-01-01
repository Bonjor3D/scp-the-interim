using Sandbox;
using System;
using System.Diagnostics;

public sealed partial class NPCController
{
    #region Properties

    [Property, Feature("View")] public float Fov = 120f;
    [Property, Feature("View")] public float MaxViewDistance = 1000f;

	#endregion

    #region Methods
    public void UpdateView()
    {
        var playerController = Scene.GetAllComponents<PlayerController>().FirstOrDefault();
        if ( playerController == null )
            return;

        var targetActor = playerController.GameObject.GetComponent<Actor>();
        if ( targetActor == null )
            return;


        if ( IsTargetVisible(targetActor) )
            Log.Info("I see you!");
    }

    #endregion

    #region LogicMethods
    private float CalcDistanceToTarget(Actor target) {return Vector3.DistanceBetween(Body.WorldPosition, target.Body.WorldPosition);}
    private bool IsTargetOnDistance(Actor target) {return CalcDistanceToTarget(target) <= MaxViewDistance;}
    private bool IsRaycastToTarget( Actor target )
    {
        var from = GetActorPartByTag("face")?.WorldPosition
                ?? Transform.Position + Vector3.Up * 64;

        var to = target.GetActorPartByTag("body")?.WorldPosition
                ?? target.Transform.Position + Vector3.Up * 48;

        var trace = Scene.Trace
            .Ray( from, to )
            .IgnoreGameObjectHierarchy( GameObject )    
            .WithoutTags( "face" )
            .UseHitboxes( true )
            .Run();

        if ( !trace.Hit )
            return false;


        

        return trace.GameObject
            .GetComponentInParent<Actor>() == target;
    }

    private bool IsTargetInFov( Actor target )
    {
        var dir = (target.Transform.Position - Body.Transform.Position).Normal;
        var dot = Vector3.Dot( Body.Transform.Rotation.Forward, dir );

        float cos = MathF.Cos( Fov * 0.5f * MathF.PI / 180f );
        return dot >= cos;
    }
    private bool IsTargetVisible( Actor target )
    {
        
        if ( !IsTargetInFov(target) )
            return false;

        if ( !IsTargetOnDistance(target) )
            return false;

        if ( !IsRaycastToTarget(target) )
            return false;
        
        return true;
    }
    #endregion
}