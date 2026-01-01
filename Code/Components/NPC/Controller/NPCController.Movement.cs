using Sandbox;

public sealed partial class NPCController
{
	public override void UpdateMovement()
	{

	}

	public void GoToMarkById( string markId )
	{
		var mark = MarkManager.GetMarkById(markId);

		if ( mark == null )
		{
			Log.Error($"Mark '{markId}' not found");
			return;
		}

		if ( navAgent == null )
		{
			Log.Error("NavMeshAgent is NULL");
			return;
		}

		navAgent.MoveTo( mark.Transform.Position );
	}

	public void GoToMark( Mark navMark )
	{
		if ( navMark == null )
		{
			Log.Error("navMark is NULL");
			return;
		}

		if ( navAgent == null )
		{
			Log.Error("NavMeshAgent is NULL");
			return;
		}

		navAgent.MoveTo( navMark.Transform.Position );
	}

}
