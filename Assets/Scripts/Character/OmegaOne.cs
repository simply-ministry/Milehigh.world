using System;

/// <summary>
/// Placeholder class for Sky.ix's creation, Omega.one.
/// Inherits from the base Character class to be a valid entity in the game world.
/// </summary>
public class OmegaOne : Character
{
    /// <summary>
    /// To link back to the Guid of its creator, Sky.ix.
    /// </summary>
    public Guid CreatorId { get; set; }

    protected override void Awake()
    {
        base.Awake();
        characterName = "Omega.one";
    }
}