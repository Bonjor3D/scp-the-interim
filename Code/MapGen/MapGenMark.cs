public sealed partial class MapGenMark : Component
{
    [Property] public MapGenType GenType { get; set; }

}

public enum MapGenType
{
    Room,
    Decor
}
