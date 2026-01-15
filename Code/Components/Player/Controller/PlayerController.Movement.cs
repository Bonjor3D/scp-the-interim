using Sandbox;

public sealed partial class PlayerController
{

	public override void UpdateMovement()
	{
		ReadInput();
		base.BuildVelocity();
		base.Move();

		if(VelocitySP > 0) {smr.Set("IsWalking", true); smr.Set("Speed", (VelocitySP / WalkSpeed));} else {smr.Set("IsWalking", false); smr.Set("Speed", 1.0f);}
	}

	public void ReadInput()
	{
		WantsToCrouch = Input.Down( "crouch" );
		WantsToRun = Input.Down( "run" );
		WantsToSlowWalk = Input.Down( "slowMove" );
		WantsToGoForward = Input.Down( "moveForward" );
		WantsToGoBackward = Input.Down( "moveBackward" );
		WantsToGoLeft = Input.Down( "moveLeft" );
		WantsToGoRight = Input.Down( "moveRight" );
	}
}
