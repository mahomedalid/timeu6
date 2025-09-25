using TimeU6.Models;

namespace TimeU6.Services;

/// <summary>
/// Interface for match state storage operations
/// </summary>
public interface IMatchStateStorage
{
    /// <summary>
    /// Saves the current match state to browser local storage
    /// </summary>
    /// <param name="matchState">The match state to save</param>
    Task SaveMatchState(MatchState matchState);
    
    /// <summary>
    /// Loads the match state from browser local storage
    /// </summary>
    /// <returns>The loaded match state or a new instance if none exists</returns>
    Task<MatchState> LoadMatchState();
    
    /// <summary>
    /// Clears the match state from browser local storage
    /// </summary>
    Task ClearMatchState();
    
    /// <summary>
    /// Checks if match state exists in browser local storage
    /// </summary>
    /// <returns>True if match state exists, false otherwise</returns>
    Task<bool> HasSavedMatchState();
}