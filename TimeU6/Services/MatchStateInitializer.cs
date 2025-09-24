using TimeU6.Models;

namespace TimeU6.Services;

/// <summary>
/// Service to handle loading saved match state at application startup
/// </summary>
public class MatchStateInitializer
{
    private readonly MatchState _matchState;
    private readonly IMatchStateStorage _storage;

    public MatchStateInitializer(MatchState matchState, IMatchStateStorage storage)
    {
        _matchState = matchState;
        _storage = storage;
    }

    /// <summary>
    /// Attempts to load saved match state from localStorage
    /// </summary>
    /// <returns>True if state was loaded, false if no saved state exists</returns>
    public async Task<bool> LoadSavedStateAsync()
    {
        try
        {
            Console.WriteLine("Checking for saved match state in local storage");
            if (await _storage.HasSavedMatchState())
            {
                var savedState = await _storage.LoadMatchState();
                
                // Copy saved state to the singleton instance
                _matchState.AllPlayers.Clear();
                _matchState.AllPlayers.AddRange(savedState.AllPlayers);
                _matchState.MatchStartTime = savedState.MatchStartTime;
                _matchState.IsMatchActive = savedState.IsMatchActive;
                _matchState.MatchDuration = savedState.MatchDuration;
                
                Console.WriteLine($"Loaded saved match state with {_matchState.AllPlayers.Count} players");
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading saved match state: {ex.Message}");
        }
        
        Console.WriteLine("No saved match state found, starting fresh");
        return false;
    }
    
    /// <summary>
    /// Clears all saved match state
    /// </summary>
    public async Task ClearSavedStateAsync()
    {
        try
        {
            await _storage.ClearMatchState();
            
            // Reset the current state
            _matchState.AllPlayers.Clear();
            _matchState.MatchStartTime = null;
            _matchState.IsMatchActive = false;
            _matchState.MatchDuration = TimeSpan.FromMinutes(30);
            
            Console.WriteLine("Cleared saved match state");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing saved match state: {ex.Message}");
            throw;
        }
    }
}