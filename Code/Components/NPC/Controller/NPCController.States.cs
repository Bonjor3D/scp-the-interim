using Sandbox;

public sealed partial class NPCController
{
    #region Parameters
    public enum NPCStates
    {
        Idle,
        Noticed,
        Suspicious,
        Searching,
        Alerted,
        Combat
    }
    #endregion
}