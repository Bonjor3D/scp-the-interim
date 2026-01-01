using Sandbox;
using System;

public abstract partial class Actor
{
	#region Stamina
	[Property, Feature("Stamina")] public float MaxStamina = 100f;
	[Property, Feature("Stamina")] public float StaminaDrainRun = 15f;
	[Property, Feature("Stamina")] public float StaminaRegen = 10f;

    [Property, Feature("Stamina")] public float RegenDelayAfterEmpty = 2.0f;

    private float regenCooldown;
	public float CurrentStamina;
	#endregion

    public void UpdateStamina()
    {

        if ( WantsToRun && CurrentStamina > 0 )
        {
            IsRunning = true;
            CurrentStamina -= StaminaDrainRun * Time.Delta;

            if ( CurrentStamina <= 0 )
            {
                CurrentStamina = 0;
                IsRunning = false;
                regenCooldown = RegenDelayAfterEmpty;
            }
        }
        else
        {
            IsRunning = false;

            if ( regenCooldown > 0 )
            {
                regenCooldown -= Time.Delta;
                return;
            }

            float regenMultiplier = GetStaminaRegenMultiplier();

            CurrentStamina = MathF.Min(
                CurrentStamina + (StaminaRegen * regenMultiplier * Time.Delta),
                MaxStamina
            );
        }
    }


    float GetStaminaRegenMultiplier()
    {
        float speed = Velocity.Length;

        if ( IsCrouching )
        {
            if ( speed > 0.1f )
                return 1.5f;
            return 2.0f;
        }

        if ( speed < 0.1f )
            return 1.0f;

        if ( IsSlowWalking && speed > 0.1f )
            return 1.2f;

        return 0.8f;
    }

}
