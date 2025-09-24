using TimeU6.Models;

namespace TimeU6.Models;

/// <summary>
/// Represents the current state of a match including players and timing information
/// </summary>
public class MatchState
{
    /// <summary>
    /// All players in the team roster
    /// </summary>
    public List<Player> AllPlayers { get; set; } = new();
    
    /// <summary>
    /// Players currently on the field
    /// </summary>
    public List<Player> PlayingPlayers => AllPlayers.Where(p => p.IsPlaying).ToList();
    
    /// <summary>
    /// Players on the bench (present but not playing)
    /// </summary>
    public List<Player> BenchPlayers => AllPlayers.Where(p => p.IsPresent && !p.IsPlaying).ToList();
    
    /// <summary>
    /// When the match started
    /// </summary>
    public DateTime? MatchStartTime { get; set; }
    
    /// <summary>
    /// Whether the match is currently active (timer running)
    /// </summary>
    public bool IsMatchActive { get; set; } = false;
    
    /// <summary>
    /// Total duration of the match
    /// </summary>
    public TimeSpan MatchDuration { get; set; } = TimeSpan.FromMinutes(30);
    
    /// <summary>
    /// Gets the elapsed time since match start
    /// </summary>
    /// <returns>Elapsed time or zero if match hasn't started</returns>
    public TimeSpan GetElapsedTime()
    {
        if (MatchStartTime.HasValue)
        {
            return DateTime.Now - MatchStartTime.Value;
        }
        return TimeSpan.Zero;
    }
    
    /// <summary>
    /// Gets the remaining match time
    /// </summary>
    /// <returns>Remaining time or zero if match is over</returns>
    public TimeSpan GetRemainingTime()
    {
        var elapsed = GetElapsedTime();
        var remaining = MatchDuration - elapsed;
        return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
    }
}