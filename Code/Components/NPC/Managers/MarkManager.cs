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
        
        if ( npcName != "all" ) {

            var npc = NPCManager.GetNPCByName(npcName);
            npc.nav.GoToMarkById(markId, npc.navAgent);
            npc.currentMark = GetMarkById(markId);

        } else {

            foreach(NPCController npc in Game.ActiveScene.GetAllComponents<NPCController>() ){
                Log.Info($"{GetMarkById(markId)}, {npc.navAgent}, {npc.nav}");
            npc.nav.GoToMarkById(markId, npc.navAgent);
            npc.currentMark = GetMarkById(markId);}

        }
    }
}
