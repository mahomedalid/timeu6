using Microsoft.JSInterop;

namespace TimeU6.Services;

/// <summary>
/// Helper service for browser local storage operations using JavaScript interop
/// </summary>
public class LocalStorageHelper : ILocalStorageHelper
{
    private readonly IJSRuntime _jsRuntime;

    public LocalStorageHelper(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <inheritdoc />
    public async Task SetItem(string key, string value)
    {
        await _jsRuntime.InvokeVoidAsync("LocalStorageActions.setItem", key, value);
    }

    /// <inheritdoc />
    public async Task<string?> GetItem(string key)
    {
        return await _jsRuntime.InvokeAsync<string?>("LocalStorageActions.getItem", key);
    }

    /// <inheritdoc />
    public async Task RemoveItem(string key)
    {
        await _jsRuntime.InvokeVoidAsync("LocalStorageActions.removeItem", key);
    }

    /// <inheritdoc />
    public async Task ClearData()
    {
        await _jsRuntime.InvokeVoidAsync("LocalStorageActions.clearData");
    }
}