using Sandbox;

public static class MarkManager
{
    private static List<Mark> marks = new();

    public static void Register( Mark mark )
    {
        Log.Info($"register a mark with id {mark.markId} and coordinates {mark.coordinates}  {mark}");
        marks.Add(mark);
    }

    public static Mark GetMarkById( string id )
    {
        Log.Info($"try to find mark {id}");
        foreach(Mark mark in Game.ActiveScene.GetAllComponents<Mark>() )
        {
            if (mark.markId == id ) return mark;
        }
        return Game.ActiveScene.GetAllComponents<Mark>().FirstOrDefault();
    }

    [ConCmd("npc_sendToMark")]
    public static void npcSendToMark( string npcName, string markId )
    {
        var npc = NPCManager.GetNPCByName(npcName);
        if ( npc == null )
        {
            Log.Error($"NPC '{npcName}' not found");
            return;
        }

        var markTo = GetMarkById(markId);
        if ( markTo == null )
        {
            Log.Error($"Mark '{markId}' not found");
            return;
        } else Log.Info(markTo.coordinates);

        npc.GoToMark(markTo);
    }

}
