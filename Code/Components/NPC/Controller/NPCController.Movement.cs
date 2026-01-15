using Sandbox;
using System;

public sealed partial class NPCController
{

	public event Action<NPCController, Mark> OnMarkReached;
	public Mark currentMark;
	private const float ReachDistance = 50f;

	public override void UpdateMovement()
	{
		CheckMarkReached();
        VelocitySP = navAgent.Velocity.Length;
        if(VelocitySP > 0) {smr.Set("IsWalking", true); smr.Set("Speed", (VelocitySP / WalkSpeed));} else {smr.Set("IsWalking", false); smr.Set("Speed", 1.0f);}
	}

	private void CheckMarkReached()
    {
        if ( currentMark == null )
            return;

        float dist = Vector3.DistanceBetween(
            WorldPosition,
            currentMark.WorldPosition
        );

        if ( dist <= ReachDistance )
        {
            var reachedMark = currentMark;
            currentMark = null;

            reachedMark.UnregisterNPC(this);
            OnMarkReached?.Invoke(this, reachedMark);
        }
    }
}
