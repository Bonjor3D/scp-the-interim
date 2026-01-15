using Sandbox;
using System;

public  partial class Actor : Component
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
	public GameObject Head;
	public GameObject Body;
	[Property] public string name;

	public SkinnedModelRenderer smr;
	public Model model;
	public Transform HeadTransform;
	public Transform BodyTransform;
	#endregion

	protected override void OnStart()
	{
		smr = Components.Get<SkinnedModelRenderer>();
		characterController = Components.Get<CharacterController>();
		model = smr.Model;

		Head = smr.GetBoneObject("Head");
		Body = smr.GetBoneObject("Hips");
	}

	protected override void OnUpdate()
	{
		UpdateMovement();
		UpdateStamina();
		UpdateStealth();

		Head = smr.GetBoneObject("Head");
		Body = smr.GetBoneObject("Hips");
	}

	protected override void OnFixedUpdate()
	{
		UpdateMovement();
		IsGrounded = characterController.IsOnGround;
	}
}
