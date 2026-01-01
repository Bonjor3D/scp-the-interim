using Sandbox;

public sealed partial class PlayerController : Actor
{

	protected override void OnUpdate()
	{
		UpdateMovement();
		UpdateCamera();
		base.UpdateStamina();
		base.UpdateStealth();
	}
}
