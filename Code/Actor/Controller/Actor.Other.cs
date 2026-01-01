using Sandbox;

public abstract partial class Actor
{
    public GameObject GetActorPartByTag(string tag )
    {
        foreach ( var child in GameObject.Children)
        {
            if ( child.Tags.Has( tag ) )
                return child;
        }

        return null;
    }
}