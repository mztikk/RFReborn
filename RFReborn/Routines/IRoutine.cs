namespace RFReborn.Routines;

/// <summary>
/// Represents an object that can execute a Tick method with cooldown inbetween
/// </summary>
public interface IRoutine : IKillable
{
    /// <summary>
    /// Tick that can get continuously called
    /// </summary>
    Task Tick();

    /// <summary>
    /// If Tick is ready
    /// </summary>
    bool Ready();

    /// <summary>
    /// Remaining Cooldown of Tick
    /// </summary>
    TimeSpan Cooldown();
}
