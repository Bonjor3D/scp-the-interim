// When player do smth. in this point create a place of Incident with any radius. It is for NPC AI.

using Sandbox;

public sealed partial class IncidentManager
{
    public float radius = 0f;
    public float creationTime = 0f;
    public enum IncidentSeverity
    {
        None = 0,
        Trivival,
        Minor,
        Serious,
        Extreme
    }
}