using Sandbox;

public sealed partial class PlayerController
{
	#region Camera Parameters
	[Property, Feature("Camera")] public float MouseSensitivity = 0.1f;
	[Property, Feature("Camera")] public bool IsYawInverted;
	[Property, Feature("Camera")] public bool IsPitchInverted;
    [Property, Feature("Camera")] public CameraComponent playerCamera;

	private float viewYaw;
	private float viewPitch;


	#endregion

	public void UpdateCamera()
	{
		float pitchSign = IsPitchInverted ? -1f : 1f;
		float yawSign   = IsYawInverted   ? -1f : 1f;

		viewPitch += Input.MouseDelta.y * pitchSign * MouseSensitivity;
		viewYaw   += Input.MouseDelta.x * yawSign   * MouseSensitivity;

		viewPitch = viewPitch.Clamp(-89.9f, 89.9f);

		WorldRotation = Rotation.FromYaw(viewYaw);

		var cameraRotation = Rotation.From(
			viewPitch,
			viewYaw,
			0f
		);

		Vector3 cameraOffset =
			Vector3.Up * 100f +
			playerCamera.WorldRotation.Forward * 20f;

		playerCamera.WorldPosition = WorldPosition + cameraOffset;



		playerCamera.WorldRotation = cameraRotation;
	}

}
