using System;
using Sandbox;

public sealed class PlayerController : Component
{
	#region Components

	#endregion

	#region Parameters

		#region Movement
			[Property, Feature("Movement"), Group("Speed")] public float WalkSpeed = 200f;
			[Property, Feature("Movement"), Group("Speed")]public float RunSpeed = 300f;
			[Property, Feature("Movement"), Group("Speed")]public float CrouchSpeed = 80f;
			[Property, Feature("Movement"), Group("Speed")] public float SlowWalkSpeed = 140f;

			[Property, Feature("Movement"), Group("Move")] public float Acceleration = 10f;
			[Property, Feature("Movement"), Group("Move")] public float Deceleration = 12f;

			[Property, Feature("Movement"), Group("Jump")] public float JumpForce = 300f;
			[Property, Feature("Movement"), Group("Jump")] public float MaxForce = 50f;

			[Property, Feature("Movement")] public float AirControl = 0.3f;
			[Property, Feature("Movement")] public float GroundControl = 4f;
		#endregion

		#region Camera
			[Property, Feature("Camera")] GameObject PlayerCamera;
			[Property, Feature("Camera")] public float MouseSensitivity = 1.0f;
			[Property, Feature("Camera")] public float CameraHeight = 64f;
			[Property, Feature("Camera")] public float CrouchCameraHeight = 40f;

			[Property, Feature("Camera")] public float CameraSwayAmount = 0.2f;
			[Property, Feature("Camera")]public float CameraBobAmount = 0.1f;

			[Property, Feature("Camera")]public bool IsYawInverted = false;
			[Property, Feature("Camera")]public bool IsPitchInverted = false;
		#endregion

		#region Stealth
			[Property, Feature("Stealth")] public float WalkNoise = 0.3f;
			[Property, Feature("Stealth")] public float RunNoise = 1.0f;
			[Property, Feature("Stealth")] public float CrouchNoise = 0.1f;
			[Property, Feature("Stealth")] public float JumpNoise = 0.8f;

			[Property, Feature("Stealth")] public float LightVisibilityMultiplier = 1.5f;
			[Property, Feature("Stealth")] public float ShadowVisibilityMultiplier = 0.5f;

			[Property, Feature("Stealth")] public float NoiseRadiusMultiplier = 1.0f;
		#endregion

		#region Stamina
			[Property, Feature("Stamina")] public float MaxStamina = 100f;
			[Property, Feature("Stamina")] public float StaminaDrainRun = 15f;
			[Property, Feature("Stamina")] public float StaminaRegen = 10f;
			[Property, Feature("Stamina")] public float StaminaRegenDelay = 1.5f;
		#endregion

		#region Interaction
			[Property, Feature("Interaction")] public float InteractionDistance = 120f;
			[Property, Feature("Interaction")] public float ThrowForce = 400f;
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

		public Vector3 Velocity = Vector3.Zero;
		private CharacterController CharacterController;
		private float pitchMltply;
		private float yawMltply;

	#endregion

	#region Methods

	protected override void OnAwake()
	{
		CharacterController = Components.Get<CharacterController>();
	}

	protected override void OnStart()
	{

	}

	protected override void OnFixedUpdate()
	{
		BuildVelocity();
		Move();

		IsGrounded = CharacterController.IsOnGround;
	}

	
	protected override void OnUpdate()
	{
		IsCrouching = Input.Down("crouch");
		IsRunning = Input.Down("run");
		IsSlowWalking = Input.Down("slowMove");

		var eyeAngle = Head.Transform.Rotation.Angles();

		if (IsPitchInverted) pitchMltply = -0.1f; else pitchMltply = 0.1f;
		if (IsYawInverted) yawMltply = 0.1f; else yawMltply = -0.1f;

		// Camera logic here
		eyeAngle.pitch += Input.MouseDelta.y * pitchMltply;
		eyeAngle.yaw += Input.MouseDelta.x * yawMltply;
		

		eyeAngle.roll = 0f;
		eyeAngle.pitch = eyeAngle.pitch.Clamp(-89.9f, 89.9f);
		Head.Transform.Rotation = eyeAngle.ToRotation();

		if (PlayerCamera is not null )
		{
			var camPos = Head.Transform.Position;
			PlayerCamera.Transform.Position = camPos;
			PlayerCamera.Transform.Rotation = Head.Transform.Rotation;
		}
	}

	[Obsolete]
	void BuildVelocity()
	{
		var wishDir = Vector3.Zero;
		var rot = Head.Transform.Rotation;

		if ( Input.Down("moveForward") ) wishDir += rot.Forward;
		if ( Input.Down("moveBackward") ) wishDir += rot.Backward;
		if ( Input.Down("moveLeft") ) wishDir += rot.Left;
		if ( Input.Down("moveRight") ) wishDir += rot.Right;

		wishDir = wishDir.WithZ(0);
		if ( !wishDir.IsNearZeroLength )
			wishDir = wishDir.Normal;

		var speed =
			IsCrouching ? CrouchSpeed :
			IsSlowWalking ? SlowWalkSpeed :
			IsRunning ? RunSpeed :
			WalkSpeed;

		Velocity = wishDir * speed;
	}

	void Move()
	{
		Vector3 gravity = Scene.PhysicsWorld.Gravity;

		if ( CharacterController.IsOnGround )
		{
			CharacterController.Velocity =
				CharacterController.Velocity.WithZ( 0 );

			CharacterController.ApplyFriction( GroundControl );
			CharacterController.Accelerate( Velocity );
		}
		else
		{
			CharacterController.Velocity += gravity * Time.Delta;
			CharacterController.Accelerate( Velocity );
			CharacterController.ApplyFriction( AirControl );
		}

		CharacterController.Move();
	}

	#endregion

	
}	