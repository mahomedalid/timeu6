using System.Text.Json;
using TimeU6.Models;

namespace TimeU6.Services;

/// <summary>
/// Service for persisting match state to browser local storage using JSON serialization
/// </summary>
public class MatchStateStorage : IMatchStateStorage
{
    private readonly ILocalStorageHelper _localStorage;
    private const string MATCH_STATE_KEY = "timeU6_matchState";
    
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        PropertyNameCaseInsensitive = true
    };

    public MatchStateStorage(ILocalStorageHelper localStorage)
    {
        _localStorage = localStorage;
    }

    /// <inheritdoc />
    public async Task SaveMatchState(MatchState matchState)
    {
        try
        {
            var json = JsonSerializer.Serialize(matchState, _jsonOptions);
            await _localStorage.SetItem(MATCH_STATE_KEY, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving match state: {ex.Message}");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<MatchState> LoadMatchState()
    {
        try
        {
            var json = await _localStorage.GetItem(MATCH_STATE_KEY);
            
            if (string.IsNullOrEmpty(json))
            {
                return new MatchState();
            }

            var matchState = JsonSerializer.Deserialize<MatchState>(json, _jsonOptions);
            return matchState ?? new MatchState();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading match state: {ex.Message}");
            // Return new instance if deserialization fails
            return new MatchState();
        }
    }

    /// <inheritdoc />
    public async Task ClearMatchState()
    {
        try
        {
            await _localStorage.RemoveItem(MATCH_STATE_KEY);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing match state: {ex.Message}");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> HasSavedMatchState()
    {
        try
        {
            var json = await _localStorage.GetItem(MATCH_STATE_KEY);
            return !string.IsNullOrEmpty(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking for saved match state: {ex.Message}");
            return false;
        }
    }
}