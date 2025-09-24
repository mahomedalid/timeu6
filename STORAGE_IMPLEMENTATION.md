# TimeU6 Match State Storage Implementation

This document describes the serialization/deserialization approach implemented for storing match state in browser localStorage.

## Overview

The implementation provides automatic persistence of match state to browser localStorage using JSON serialization. All match state changes are automatically saved, and the state is restored when the application loads.

## Architecture Components

### 1. JavaScript Layer (`wwwroot/js/LocalStorage.js`)
```javascript
window.LocalStorageActions = {
    setItem: function (key, value) {
        localStorage.setItem(key, value);
    },
    getItem: function (key) {
        return localStorage.getItem(key);
    },
    removeItem: function (key) {
        localStorage.removeItem(key);
    },
    clearData: function () {
        localStorage.clear();
    }
}
```

### 2. C# Wrapper Service
- **`ILocalStorageHelper`** - Interface for browser localStorage operations
- **`LocalStorageHelper`** - Implementation using JSInterop to call JavaScript functions

### 3. Match State Storage Service
- **`IMatchStateStorage`** - Interface for match state persistence operations
- **`MatchStateStorage`** - Implementation using JSON serialization/deserialization

### 4. Initializer Service
- **`MatchStateInitializer`** - Service to load saved state at application startup

## Key Features

### Automatic Persistence
All services (`PlayerService` and `MatchService`) automatically save state changes to localStorage:
- Adding/removing players
- Updating player presence
- Starting/pausing/resuming matches
- Player substitutions
- Field assignments

### JSON Serialization
Uses `System.Text.Json` with these options:
```csharp
var jsonOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = false,
    PropertyNameCaseInsensitive = true
};
```

### Error Handling
- Graceful degradation when localStorage is unavailable
- Console logging for debugging
- Automatic fallback to fresh state on deserialization errors

### Background Saving
Auto-save operations run in background tasks to avoid blocking UI:
```csharp
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
```

## Usage

### 1. Service Registration (Program.cs)
```csharp
// Register local storage services
builder.Services.AddScoped<ILocalStorageHelper, LocalStorageHelper>();
builder.Services.AddScoped<IMatchStateStorage, MatchStateStorage>();
builder.Services.AddScoped<MatchStateInitializer>();
```

### 2. Script Reference (index.html)
```html
<script src="js/LocalStorage.js"></script>
```

### 3. Loading Saved State in Components
```csharp
@inject MatchStateInitializer StateInitializer

protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (firstRender)
    {
        await StateInitializer.LoadSavedStateAsync();
        StateHasChanged();
    }
}
```

### 4. Manual Storage Operations
```csharp
@inject IMatchStateStorage Storage

// Save current state
await Storage.SaveMatchState(matchState);

// Load saved state
var savedState = await Storage.LoadMatchState();

// Check if saved state exists
var hasSaved = await Storage.HasSavedMatchState();

// Clear saved state
await Storage.ClearMatchState();
```

## Storage Key

The match state is stored in localStorage with the key: `"timeU6_matchState"`

## Browser Compatibility

This implementation works in all modern browsers that support:
- localStorage API
- WebAssembly (for Blazor WebAssembly)
- ES6 JavaScript features

## Data Structure

The serialized JSON includes:
```json
{
    "allPlayers": [
        {
            "id": "guid",
            "name": "string",
            "number": 0,
            "isPresent": true,
            "isPlaying": false,
            "playingTime": "00:00:00",
            "playingStartTime": null
        }
    ],
    "matchStartTime": null,
    "isMatchActive": false,
    "matchDuration": "00:30:00"
}
```

## Security Considerations

- Data is stored in browser localStorage (client-side only)
- No sensitive information should be stored in match state
- Data persists until explicitly cleared or browser data is cleared
- Consider data privacy regulations when storing player information

## Performance

- JSON serialization/deserialization is performed asynchronously
- Storage operations run in background tasks
- Minimal impact on UI responsiveness
- Data size is typically small (a few KB for typical team rosters)

## Testing

To test the storage functionality:
1. Create players and start a match
2. Refresh the browser - state should be restored
3. Open browser DevTools > Application > Local Storage to view stored data
4. Use the `StorageManager` component to manually test storage operations

## Troubleshooting

### Common Issues:
1. **"LocalStorageActions is not defined"** - Ensure `LocalStorage.js` is loaded
2. **State not persisting** - Check browser console for JavaScript errors
3. **Deserialization errors** - Stored data may be corrupted; clear localStorage and restart

### Debug Information:
- Check browser console for error messages
- Inspect localStorage in browser DevTools
- Use `StorageManager` component to check storage status