using System;

public static class GenLogic
{
    public static int WorldSeed { get; private set; }
    public static GameObject mainRoom;
    static List<PrefabFile> roomPrefabs;
    static bool prefabsLoaded;
    const int MaxDepth = 7;

    [ConCmd("SetGenSeedTo")]
    public static void SetSeed(string input)
    {
        if (!int.TryParse(input, out int parsed))
        {
            parsed = input.GetHashCode();
        }

        WorldSeed = parsed;
        Log.Info($"WorldSeed set to {WorldSeed}");
    }

    public static int GetRandomInt(
        int min,
        int max,
        int contextSeed
    )
    {
        int combinedSeed = Hash(WorldSeed, contextSeed);
        Random rng = new Random(combinedSeed);
        return rng.Int(min, max);
    }

    public static int GetRandomInt(
        int min,
        int max,
        string context
    )
    {
        return GetRandomInt(min, max, context.GetHashCode());
    }

    public static float GetRandomFloat(
        float min,
        float max,
        int contextSeed
    )
    {
        int combinedSeed = Hash(WorldSeed, contextSeed);
        Random rng = new Random(combinedSeed);
        return rng.Float(min, max);
    }

    private static int Hash(int a, int b)
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 31 + a;
            hash = hash * 31 + b;
            return hash;
        }
    }

    static void LoadPrefabs()
    {
        if ( prefabsLoaded )
            return;

        roomPrefabs = ResourceLibrary
            .GetAll<PrefabFile>( "prefabs/mapgen/rooms" )
            .Where( x => !x.ResourcePath.Contains( "room_main" ) )
            .ToList();

        prefabsLoaded = true;
    }


    static void GenerateFromRoom( GameObject room, int depth )
    {
        Random random = new Random(room.Id.GetHashCode());

        if ( depth >= MaxDepth )
            return;

        var marker = FindNextRoomMarker( room );
        if ( marker == null )
            return;

        var prefab = roomPrefabs.OrderBy( _ => random.Float() ).First();

        GameObject newRoom = GameObject.Clone(prefab);

        newRoom.Transform.Position = marker.Transform.Position;
        newRoom.Transform.Rotation = marker.Transform.Rotation;


        var comp = newRoom.Components.GetOrCreate<MapRoom>();
        comp.Depth = depth + 1;

        GenerateFromRoom( newRoom, comp.Depth );
    }

    static GameObject FindNextRoomMarker( GameObject root )
    {
        if ( root.Name == "NextRoomMark" )
            return root;

        foreach ( var child in root.Children )
        {
            var result = FindNextRoomMarker( child );
            if ( result != null )
                return result;
        }

        return null;
    }

    public static void Initialize()
    {
        Log.Info("Main_Room is not null!");
        mainRoom = Game.ActiveScene.Children
            .FirstOrDefault( x => x.Name == "Room_Main" );

        if ( mainRoom == null )
        {
            Log.Error( "Room_Main not found!" );
            return;
        }
    }

    [ConCmd("GenerateMapTest")]
    public static void GenerateMap()
    {
        Log.Info("Starting generating...");
        LoadPrefabs();
        Initialize();

        var roomComp = mainRoom.Components.GetOrCreate<MapRoom>();
        roomComp.Depth = 0;

        GenerateFromRoom( mainRoom, roomComp.Depth );
    }
}
