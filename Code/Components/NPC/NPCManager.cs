using Sandbox;

public static class NPCManager
{
    public static NPCController GetNPCByName( string name )
    {
        Log.Info("Start finding!");
        foreach (NPCController npc in Game.ActiveScene.GetAllComponents<NPCController>())
        {
            Log.Info(npc);
            if (npc.npcName == name) return npc;
        } return Game.ActiveScene.GetComponents<NPCController>().FirstOrDefault();
        
    }
}
