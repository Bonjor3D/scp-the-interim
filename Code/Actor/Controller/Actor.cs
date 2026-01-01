using Sandbox;

public abstract partial class Actor : Component
{
	#region Components
	public CharacterController characterController;
	#endregion

	#region State
	public bool IsGrounded;
	public bool IsCrouching;
	public bool IsRunning;
	public bool IsSlowWalking;
	public bool IsInShadow;
	#endregion

	#region General
	[Property] public GameObject Head;
	[Property] public GameObject Body;
	#endregion

	protected override void OnAwake()
	{
		characterController = Components.Get<CharacterController>();
		Head = GetActorPartByTag("head");
		Body = GetActorPartByTag("body");
	}

	protected override void OnUpdate()
	{
		UpdateMovement();
		UpdateStamina();
		UpdateStealth();
	}

	protected override void OnFixedUpdate()
	{
		UpdateMovement();
		IsGrounded = characterController.IsOnGround;
	}
}
