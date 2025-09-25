using TimeU6.Models;

namespace TimeU6.Services;

/// <summary>
/// Interface for managing player operations
/// </summary>
public interface IPlayerService
{
    /// <summary>
    /// Gets all players in the team
    /// </summary>
    /// <returns>List of all players</returns>
    List<Player> GetAllPlayers();
    
    /// <summary>
    /// Adds a new player to the team
    /// </summary>
    /// <param name="name">Player's name</param>
    /// <returns>The created player</returns>
    Player AddPlayer(string name);
    
    /// <summary>
    /// Removes a player from the team
    /// </summary>
    /// <param name="playerId">ID of the player to remove</param>
    /// <returns>True if player was removed, false if not found</returns>
    bool RemovePlayer(Guid playerId);
    
    /// <summary>
    /// Updates a player's presence status
    /// </summary>
    /// <param name="playerId">ID of the player</param>
    /// <param name="isPresent">Whether the player is present</param>
    /// <returns>True if updated successfully, false if player not found</returns>
    bool UpdatePlayerPresence(Guid playerId, bool isPresent);
    
    /// <summary>
    /// Gets a player by their ID
    /// </summary>
    /// <param name="playerId">ID of the player</param>
    /// <returns>The player if found, null otherwise</returns>
    Player? GetPlayer(Guid playerId);
    
    /// <summary>
    /// Updates a player's jersey number
    /// </summary>
    /// <param name="playerId">ID of the player</param>
    /// <param name="number">New jersey number</param>
    /// <returns>True if updated successfully, false if player not found or number already taken</returns>
    bool UpdatePlayerNumber(Guid playerId, int number);
}