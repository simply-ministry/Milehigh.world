/// <summary>
/// Placeholder class for the personification of the Void, Era.
/// Inherits from the base Character class to be a valid entity in the game world.
/// </summary>
public class Era : Character
{
    protected override void Awake()
    {
        base.Awake();
        characterName = "Era, the Corrupted Void";
    }
}