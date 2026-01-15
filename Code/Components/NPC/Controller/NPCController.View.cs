using Sandbox;
using System;

public sealed partial class NPCController
{
    #region Properties
    [Property, Feature("View")] public float Fov = 120f;
    [Property, Feature("View")] public float MaxViewDistance = 1000f;
    [Property, Feature("View")] public float MaxViewDistanceInShadow = 100f;
	#endregion

    #region Events
    public event Action<NPCController, Actor> OnTargetSpotted;
    public event Action<NPCController, Actor> OnTargetLost;
    private HashSet<Actor> visibleActors = new();
    #endregion

    #region Methods
    public void UpdateView()
    {
        var currentlyVisible = new HashSet<Actor>();

        foreach ( var actor in Scene.GetAllComponents<Actor>() )
        {
            if ( actor == this )
                continue;

            if ( IsTargetVisible(actor) )
            {
                currentlyVisible.Add(actor);

                if ( !visibleActors.Contains(actor) )
                {
                    visibleActors.Add(actor);
                    OnTargetSpotted?.Invoke(this, actor);

                    Log.Info($"{this.name}: [SPOTTED] {actor}");
                }
            }
        }

        foreach ( var actor in visibleActors.ToArray() )
        {
            if ( !currentlyVisible.Contains(actor) )
            {
                visibleActors.Remove(actor);
                OnTargetLost?.Invoke(this, actor);

                Log.Info($"[LOST] {actor.GetType().Name}");
            }
        }
    }



    #endregion

    #region LogicMethods
    private float CalcDistanceToTarget(Actor target) {return Vector3.DistanceBetween(Body.WorldPosition, target.Body.WorldPosition);}
    private bool IsTargetOnDistance(Actor target) {return CalcDistanceToTarget(target) <= MaxViewDistance;}
    private bool IsTargetOnShadow(Actor target) {return target.IsInShadow;}
    private bool IsRaycastToTarget( Actor target )
    {
        var from = Head.WorldPosition;

        var to = target.Body.WorldPosition;

        var trace = Scene.Trace
            .Ray( from, to )
            .IgnoreGameObjectHierarchy( GameObject )
            .UseHitboxes( true )
            .Run();

        if ( !trace.Hit )
            return false;

        return trace.GameObject
            .GetComponentInParent<Actor>() == target;
    }

    private bool IsTargetInFov( Actor target )
    {
        var dir = (target.Body.WorldPosition - WorldPosition).Normal;
        var dot = Vector3.Dot( WorldRotation.Forward, dir );

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

        if ( IsTargetOnShadow(target) )
            if ( CalcDistanceToTarget(target) <  MaxViewDistanceInShadow) {return true;} else return false;
        
        return true;
    }
    #endregion
}