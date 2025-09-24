namespace TimeU6.Models;

/// <summary>
/// Represents a player in the soccer team with basic properties for match management
/// </summary>
public class Player
{
    /// <summary>
    /// Unique identifier for the player
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// Player's name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Player's jersey number
    /// </summary>
    public int Number { get; set; }
    
    /// <summary>
    /// Whether the player is present for the match
    /// </summary>
    public bool IsPresent { get; set; } = true;
    
    /// <summary>
    /// Whether the player is currently playing on the field
    /// </summary>
    public bool IsPlaying { get; set; } = false;
    
    /// <summary>
    /// Total playing time accumulated during the match
    /// </summary>
    public TimeSpan PlayingTime { get; set; } = TimeSpan.Zero;
    
    /// <summary>
    /// The time when the player started playing (if currently on field)
    /// </summary>
    public DateTime? PlayingStartTime { get; set; }
}