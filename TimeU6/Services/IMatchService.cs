using TimeU6.Models;

namespace TimeU6.Services;

/// <summary>
/// Interface for managing match operations
/// </summary>
public interface IMatchService
{
    /// <summary>
    /// Starts the match timer
    /// </summary>
    void StartMatch();
    
    /// <summary>
    /// Pauses the match timer
    /// </summary>
    void PauseMatch();
    
    /// <summary>
    /// Resumes the match timer
    /// </summary>
    void ResumeMatch();
    
    /// <summary>
    /// Resets the match timer
    /// </summary>
    void ResetMatch();
    
    /// <summary>
    /// Substitutes a player (brings one in and takes one out)
    /// </summary>
    /// <param name="playerInId">ID of player going on the field</param>
    /// <param name="playerOutId">ID of player coming off the field</param>
    /// <returns>True if substitution was successful</returns>
    bool SubstitutePlayer(Guid playerInId, Guid playerOutId);
    
    /// <summary>
    /// Adds a player to the field (if space available)
    /// </summary>
    /// <param name="playerId">ID of player to add</param>
    /// <returns>True if player was added to field</returns>
    bool AddPlayerToField(Guid playerId);
    
    /// <summary>
    /// Removes a player from the field
    /// </summary>
    /// <param name="playerId">ID of player to remove</param>
    /// <returns>True if player was removed from field</returns>
    bool RemovePlayerFromField(Guid playerId);
    
    /// <summary>
    /// Gets the elapsed match time
    /// </summary>
    /// <returns>Elapsed time since match started</returns>
    TimeSpan GetElapsedTime();
    
    /// <summary>
    /// Checks if another player can be added to the field
    /// </summary>
    /// <returns>True if field has space for another player</returns>
    bool CanAddPlayerToField();
    
    /// <summary>
    /// Gets the maximum number of players allowed on field
    /// </summary>
    /// <returns>Maximum players on field (6 for U6 soccer)</returns>
    int GetMaxPlayersOnField();
    
    /// <summary>
    /// Updates playing time for all active players
    /// </summary>
    void UpdatePlayingTimes();
    
    /// <summary>
    /// Initializes a new match with present players, optionally starting some players on field
    /// </summary>
    /// <param name="startWithPlayers">Whether to automatically start players on field</param>
    /// <returns>True if match was initialized successfully</returns>
    bool InitializeMatch(bool startWithPlayers = false);
}