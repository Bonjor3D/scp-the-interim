using Sandbox;

public class Mark : Component
{
    [Property] public string markId;

    public Vector3 coordinates;

    protected override void OnStart()
    {
        coordinates = this.Transform.Position;
        MarkManager.Register(this);
    }
}