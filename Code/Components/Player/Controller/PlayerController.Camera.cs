using Sandbox;

public sealed partial class PlayerController
{
	#region Camera Parameters
	[Property, Feature("Camera")] public float MouseSensitivity = 1.0f;
	[Property, Feature("Camera")] public bool IsYawInverted;
	[Property, Feature("Camera")] public bool IsPitchInverted;
    [Property, Feature("Camera")] public CameraComponent playerCamera;
	#endregion

	public void UpdateCamera()
	{
		var eyeAngles = Head.Transform.Rotation.Angles();

		float pitchSign = IsPitchInverted ? -0.1f : 0.1f;
		float yawSign   = IsYawInverted   ?  0.1f : -0.1f;

		eyeAngles.pitch += Input.MouseDelta.y * pitchSign * MouseSensitivity;
		eyeAngles.yaw   += Input.MouseDelta.x * yawSign   * MouseSensitivity;

		eyeAngles.roll = 0f;
		eyeAngles.pitch = eyeAngles.pitch.Clamp( -89.9f, 89.9f );

		Head.Transform.Rotation = eyeAngles.ToRotation();

        playerCamera.Transform.Position = Head.Transform.Position;
        playerCamera.Transform.Rotation = Head.Transform.Rotation;
	}
}
