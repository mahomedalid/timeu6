using TimeU6.Models;

namespace TimeU6.Services;

/// <summary>
/// Implementation of player management service with automatic local storage persistence
/// </summary>
public class PlayerService : IPlayerService
{
    private readonly MatchState _matchState;
    private readonly IMatchStateStorage _storage;
    
    public PlayerService(MatchState matchState, IMatchStateStorage storage)
    {
        Console.WriteLine("Initializing player service");
        _matchState = matchState;
        _storage = storage;
    }
    
    /// <inheritdoc />
    public List<Player> GetAllPlayers()
    {
        return _matchState.AllPlayers.ToList();
    }
    
    /// <inheritdoc />
    public Player AddPlayer(string name)
    {
        Console.WriteLine($"Adding player with name: {name}");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Player name cannot be empty", nameof(name));
        
        var player = new Player
        {
            Name = name.Trim(),
            Number = GetNextAvailableNumber()
        };
        
        _matchState.AllPlayers.Add(player);
        
        // Auto-save to localStorage
        _ = Task.Run(async () =>
        {
            try
            {
                Console.WriteLine($"Auto-saving match state after adding player {player.Name}");
                await _storage.SaveMatchState(_matchState);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error auto-saving match state: {ex.Message}");
            }
        });
        
        return player;
    }
    
    /// <inheritdoc />
    public bool RemovePlayer(Guid playerId)
    {
        var player = _matchState.AllPlayers.FirstOrDefault(p => p.Id == playerId);
        if (player == null)
            return false;
        
        var removed = _matchState.AllPlayers.Remove(player);
        
        if (removed)
        {
            // Auto-save to localStorage
            _ = Task.Run(async () =>
            {
                try
                {
                    await _storage.SaveMatchState(_matchState);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error auto-saving match state: {ex.Message}");
                }
            });
        }
        
        return removed;
    }
    
    /// <inheritdoc />
    public bool UpdatePlayerPresence(Guid playerId, bool isPresent)
    {
        var player = _matchState.AllPlayers.FirstOrDefault(p => p.Id == playerId);
        if (player == null)
            return false;
        
        player.IsPresent = isPresent;
        
        // If player is marked absent and currently playing, remove from field
        if (!isPresent && player.IsPlaying)
        {
            player.IsPlaying = false;
            player.PlayingStartTime = null;
        }
        
        // Auto-save to localStorage
        _ = Task.Run(async () =>
        {
            try
            {
                await _storage.SaveMatchState(_matchState);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error auto-saving match state: {ex.Message}");
            }
        });
        
        return true;
    }
    
    /// <inheritdoc />
    public Player? GetPlayer(Guid playerId)
    {
        return _matchState.AllPlayers.FirstOrDefault(p => p.Id == playerId);
    }
    
    /// <inheritdoc />
    public bool UpdatePlayerNumber(Guid playerId, int number)
    {
        if (number <= 0)
            return false;
        
        var player = _matchState.AllPlayers.FirstOrDefault(p => p.Id == playerId);
        if (player == null)
            return false;
        
        // Check if number is already taken by another player
        if (_matchState.AllPlayers.Any(p => p.Id != playerId && p.Number == number))
            return false;
        
        player.Number = number;
        
        // Auto-save to localStorage
        _ = Task.Run(async () =>
        {
            try
            {
                await _storage.SaveMatchState(_matchState);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error auto-saving match state: {ex.Message}");
            }
        });
        
        return true;
    }
    
    /// <summary>
    /// Gets the next available jersey number
    /// </summary>
    /// <returns>Next available number starting from 1</returns>
    private int GetNextAvailableNumber()
    {
        var usedNumbers = _matchState.AllPlayers.Select(p => p.Number).ToHashSet();
        
        for (int i = 1; i <= 99; i++)
        {
            if (!usedNumbers.Contains(i))
                return i;
        }
        
        // Fallback if somehow all numbers 1-99 are used
        return _matchState.AllPlayers.Count + 1;
    }
}