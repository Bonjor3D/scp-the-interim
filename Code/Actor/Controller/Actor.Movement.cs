using Microsoft.VisualBasic;
using Sandbox;

public partial class Actor
{
	#region Movement Parameters
	[Property, Feature("Movement"), Group("Speed")] public float WalkSpeed = 200f;
	[Property, Feature("Movement"), Group("Speed")] public float RunSpeed = 300f;
	[Property, Feature("Movement"), Group("Speed")] public float CrouchSpeed = 80f;
	[Property, Feature("Movement"), Group("Speed")] public float SlowWalkSpeed = 140f;

	[Property, Feature("Movement")] public float AirControl = 0.3f;
	[Property, Feature("Movement")] public float GroundControl = 4f;
	#endregion

	public Vector3 Velocity;
	public float VelocitySP;

	protected bool WantsToCrouch;
	protected bool WantsToRun;
	protected bool WantsToSlowWalk;
	protected bool WantsToGoForward;
	protected bool WantsToGoBackward;
	protected bool WantsToGoLeft;
	protected bool WantsToGoRight;
	private AnimParam<bool> AnimIsWalking;

	public virtual void UpdateMovement()
	{
		ApplyMovementSatate();
		BuildVelocity();
		Move();
	}

	private void ApplyMovementSatate()
	{
		IsCrouching = WantsToCrouch;
		IsRunning = WantsToRun;
		IsSlowWalking = WantsToSlowWalk;
	}

	public void BuildVelocity()
	{
		var wishDir = Vector3.Zero;

		var rot = WorldRotation;

		if (WantsToGoForward) wishDir += rot.Forward;
		if (WantsToGoBackward) wishDir += rot.Backward;
		if (WantsToGoLeft) wishDir += rot.Left;
		if (WantsToGoRight) wishDir += rot.Right;

		wishDir = wishDir.WithZ(0);
		if ( !wishDir.IsNearZeroLength )
			wishDir = wishDir.Normal;

		float speed =
			IsCrouching ? CrouchSpeed :
			IsSlowWalking ? SlowWalkSpeed :
			IsRunning ? RunSpeed :
			WalkSpeed;

		Velocity = wishDir * speed;
		VelocitySP = Velocity.Length;
        if(VelocitySP > 0) {smr.Set("IsWalking", true);} else {smr.Set("IsWalking", false);}
	}

	public void Move()
	{
		var gravity = Scene.PhysicsWorld.Gravity;

		if ( characterController.IsOnGround )
		{
			characterController.Velocity =
				characterController.Velocity.WithZ( 0 );

			characterController.ApplyFriction( GroundControl );
			characterController.Accelerate( Velocity );
		}
		else
		{
			characterController.Velocity += gravity * Time.Delta;
			characterController.Accelerate( Velocity * AirControl );
		}

		characterController.Move();
	}
}
