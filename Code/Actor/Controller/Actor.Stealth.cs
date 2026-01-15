using Sandbox;

public partial class Actor
{
    #region 

    public float PlayerNoiseLevel = 0f;

    public StealthNoiseStates CurrentState = StealthNoiseStates.silent;
    public enum StealthNoiseStates {
        silent,
        quite,
        normal,
        loud,
        critical,
        supercritical
    }
    public enum StealthVisibilityStates
    {
        invisible, //Not make any sound, in move also.
        lowVisible, //in shadow
        visible, //on light
        goodVisible, //when nps's see a player (eye in eye)
        critVisible //when is located with cams (npc will see through wall while player dont hide from cameras and npc's)
    }

    #endregion
    public void UpdateStealth()
    {
        CalcStealthNoise();
        UpdateStealthNoise();
    }

    #region Stealth logic
    private void UpdateStealthNoise()
    {
        if( PlayerNoiseLevel <= -10f) CurrentState = StealthNoiseStates.silent;
        else if(PlayerNoiseLevel >= 0f&& PlayerNoiseLevel < .2f) CurrentState = StealthNoiseStates.quite;
        else if(PlayerNoiseLevel >= .2f && PlayerNoiseLevel < .4f) CurrentState = StealthNoiseStates.normal;
        else if(PlayerNoiseLevel >= .4f && PlayerNoiseLevel < .7f) CurrentState = StealthNoiseStates.loud;
        else if(PlayerNoiseLevel >= .7f && PlayerNoiseLevel < .9f) CurrentState = StealthNoiseStates.critical;
        else if(PlayerNoiseLevel >= .9f && PlayerNoiseLevel < 1f) CurrentState = StealthNoiseStates.supercritical;
    }
    private void CalcStealthNoise()
    {
        float speed = Velocity.Length;

        if ( IsCrouching )
        {
            if ( speed > 0.1f )
                PlayerNoiseLevel = 0.2f;
            else
                PlayerNoiseLevel = 0.0f;
        }
        else if ( IsRunning && speed > 0.1f )
        {
            PlayerNoiseLevel = 0.8f;
        }
        else if ( IsSlowWalking && speed > 0.1f )
        {
            PlayerNoiseLevel = 0.4f;
        }
        else if ( speed > 0.1f )
        {
            PlayerNoiseLevel = 0.6f;
        }
        else
        {
            PlayerNoiseLevel = 0.05f;
        }
    }
    private void UpdateStealthVisibility()
    {
        // TODO: UpdateStealthVisibility, after NPC's
    }
    private void CalcStealthVisibility()
    {
        // TODO: CalcStealthVisibility, after NPC's, and maybe custom light system
    }
    #endregion
}