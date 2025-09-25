using TimeU6.Models;

namespace TimeU6.Services;

/// <summary>
/// Implementation of match management service with automatic local storage persistence
/// </summary>
public class MatchService : IMatchService
{
    private readonly MatchState _matchState;
    private readonly IMatchStateStorage _storage;
    private const int MAX_PLAYERS_ON_FIELD = 6; // U6 soccer allows 6 players on field
    
    public MatchService(MatchState matchState, IMatchStateStorage storage)
    {
        _matchState = matchState;
        _storage = storage;
    }
    
    /// <inheritdoc />
    public void StartMatch()
    {
        Console.WriteLine("Starting match");
        if (!_matchState.IsMatchActive)
        {
            Console.WriteLine("Match was not active, setting start time");
            _matchState.MatchStartTime = DateTime.Now;
            _matchState.IsMatchActive = true;
            
            // Set start times for players currently on field
            foreach (var player in _matchState.PlayingPlayers)
            {
                if (!player.PlayingStartTime.HasValue)
                {
                    player.PlayingStartTime = DateTime.Now;
                }
            }
            
            // Auto-save to localStorage
            _ = Task.Run(async () =>
            {
                try
                {
                    Console.WriteLine("Auto-saving match state after starting match");
                    await _storage.SaveMatchState(_matchState);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error auto-saving match state: {ex.Message}");
                }
            });
        }
    }
    
    /// <inheritdoc />
    public void PauseMatch()
    {
        if (_matchState.IsMatchActive)
        {
            _matchState.IsMatchActive = false;
            
            // Update playing time for all active players
            UpdatePlayingTimes();
            
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
    }
    
    /// <inheritdoc />
    public void ResumeMatch()
    {
        if (!_matchState.IsMatchActive && _matchState.MatchStartTime.HasValue)
        {
            _matchState.IsMatchActive = true;
            
            // Reset start times for players currently on field
            foreach (var player in _matchState.PlayingPlayers)
            {
                player.PlayingStartTime = DateTime.Now;
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
        }
    }
    
    /// <inheritdoc />
    public void ResetMatch()
    {
        _matchState.IsMatchActive = false;
        _matchState.MatchStartTime = null;
        
        // Reset all player states
        foreach (var player in _matchState.AllPlayers)
        {
            player.IsPlaying = false;
            player.PlayingTime = TimeSpan.Zero;
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
    }
    
    /// <inheritdoc />
    public bool SubstitutePlayer(Guid playerInId, Guid playerOutId)
    {
        var playerIn = _matchState.AllPlayers.FirstOrDefault(p => p.Id == playerInId);
        var playerOut = _matchState.AllPlayers.FirstOrDefault(p => p.Id == playerOutId);
        
        // Validate substitution
        if (playerIn == null || playerOut == null)
            return false;
        
        if (!playerIn.IsPresent || playerIn.IsPlaying)
            return false;
        
        if (!playerOut.IsPlaying)
            return false;
        
        // Perform substitution
        RemovePlayerFromField(playerOutId);
        AddPlayerToField(playerInId);
        
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
    public bool AddPlayerToField(Guid playerId)
    {
        var player = _matchState.AllPlayers.FirstOrDefault(p => p.Id == playerId);
        
        if (player == null || !player.IsPresent || player.IsPlaying)
            return false;
        
        if (!CanAddPlayerToField())
            return false;
        
        player.IsPlaying = true;
        
        if (_matchState.IsMatchActive)
        {
            player.PlayingStartTime = DateTime.Now;
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
    public bool RemovePlayerFromField(Guid playerId)
    {
        var player = _matchState.AllPlayers.FirstOrDefault(p => p.Id == playerId);
        
        if (player == null || !player.IsPlaying)
            return false;
        
        player.IsPlaying = false;
        
        // Update playing time if match is active
        if (_matchState.IsMatchActive && player.PlayingStartTime.HasValue)
        {
            player.PlayingTime += DateTime.Now - player.PlayingStartTime.Value;
        }
        
        player.PlayingStartTime = null;
        
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
    public TimeSpan GetElapsedTime()
    {
        return _matchState.GetElapsedTime();
    }
    
    /// <inheritdoc />
    public bool CanAddPlayerToField()
    {
        return _matchState.PlayingPlayers.Count < MAX_PLAYERS_ON_FIELD;
    }
    
    /// <inheritdoc />
    public int GetMaxPlayersOnField()
    {
        return MAX_PLAYERS_ON_FIELD;
    }
    
    /// <inheritdoc />
    public void UpdatePlayingTimes()
    {
        if (!_matchState.IsMatchActive)
            return;
        
        var currentTime = DateTime.Now;
        
        foreach (var player in _matchState.PlayingPlayers)
        {
            if (player.PlayingStartTime.HasValue)
            {
                var sessionTime = currentTime - player.PlayingStartTime.Value;
                player.PlayingTime += sessionTime;
                player.PlayingStartTime = currentTime; // Reset for next update
            }
        }
    }
    
    /// <inheritdoc />
    public bool InitializeMatch(bool startWithPlayers = false)
    {
        var presentPlayers = _matchState.AllPlayers.Where(p => p.IsPresent).ToList();
        
        // Need at least 3 players to start
        if (presentPlayers.Count < 3)
            return false;
        
        // Reset match state
        ResetMatch();
        
        // If requested, start some players on field
        if (startWithPlayers)
        {
            var playersToStart = presentPlayers.Take(Math.Min(presentPlayers.Count, MAX_PLAYERS_ON_FIELD)).ToList();
            foreach (var player in playersToStart)
            {
                AddPlayerToField(player.Id);
            }
        }

        StartMatch();

        // Auto-save to localStorage
        _ = Task.Run(async () =>
        {
            try
            {
                Console.WriteLine("Initializing match state and saving to storage.");
                await _storage.SaveMatchState(_matchState);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error auto-saving match state: {ex.Message}");
            }
        });
        
        return true;
    }
}