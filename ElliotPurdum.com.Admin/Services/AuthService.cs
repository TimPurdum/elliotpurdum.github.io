using Microsoft.JSInterop;

namespace ElliotPurdum.Admin.Services;

/// <summary>
/// Tracks the current Personal Access Token. Persisted to localStorage so the user
/// stays "logged in" across reloads. Validation against GitHub happens via
/// <see cref="GitHubApiService.GetAuthenticatedUserAsync"/>.
/// </summary>
public sealed class AuthService(IJSRuntime js)
{
    private const string StorageKey = "elliotpurdum.admin.pat";

    public string? Token { get; private set; }
    public GitHubUser? User { get; private set; }
    public bool IsAuthenticated => !string.IsNullOrEmpty(Token) && User is not null;

    public event Action? OnChanged;

    /// <summary>Load any previously stored token from localStorage on app startup.</summary>
    public async Task InitializeAsync(GitHubApiService api, CancellationToken ct = default)
    {
        string? stored = await js.InvokeAsync<string?>("localStorage.getItem", ct, StorageKey);
        if (string.IsNullOrEmpty(stored)) return;

        // Validate the stored token before considering ourselves logged in.
        GitHubUser? user = await api.GetAuthenticatedUserAsync(stored, ct);
        if (user is null)
        {
            // Stored token is invalid or revoked — drop it.
            await js.InvokeVoidAsync("localStorage.removeItem", ct, StorageKey);
            return;
        }
        Token = stored;
        User = user;
        OnChanged?.Invoke();
    }

    /// <summary>Validate <paramref name="token"/> and, on success, persist it.</summary>
    public async Task<bool> LoginAsync(string token, GitHubApiService api, CancellationToken ct = default)
    {
        GitHubUser? user = await api.GetAuthenticatedUserAsync(token, ct);
        if (user is null) return false;
        Token = token;
        User = user;
        await js.InvokeVoidAsync("localStorage.setItem", ct, StorageKey, token);
        OnChanged?.Invoke();
        return true;
    }

    public async Task LogoutAsync(CancellationToken ct = default)
    {
        Token = null;
        User = null;
        await js.InvokeVoidAsync("localStorage.removeItem", ct, StorageKey);
        OnChanged?.Invoke();
    }
}
