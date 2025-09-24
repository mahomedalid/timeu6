namespace TimeU6.Services;

/// <summary>
/// Interface for browser local storage operations
/// </summary>
public interface ILocalStorageHelper
{
    /// <summary>
    /// Stores a value in local storage
    /// </summary>
    /// <param name="key">The storage key</param>
    /// <param name="value">The value to store</param>
    Task SetItem(string key, string value);
    
    /// <summary>
    /// Retrieves a value from local storage
    /// </summary>
    /// <param name="key">The storage key</param>
    /// <returns>The stored value or null if not found</returns>
    Task<string?> GetItem(string key);
    
    /// <summary>
    /// Removes an item from local storage
    /// </summary>
    /// <param name="key">The storage key</param>
    Task RemoveItem(string key);
    
    /// <summary>
    /// Clears all local storage data
    /// </summary>
    Task ClearData();
}