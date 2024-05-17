

namespace Infrastructure.Services;

public class DarkModeService
{
    public event Func<Task>? OnChangeAsync;

    public bool IsDarkMode { get; private set; }

    public async Task SetDarkMode(bool isDarkMode)
    {
        if (IsDarkMode != isDarkMode)
        {
            IsDarkMode = isDarkMode;
            await NotifyStateChangedAsync();
        }
    }

    private async Task NotifyStateChangedAsync()
    {
        if (OnChangeAsync != null)
        {
            await OnChangeAsync.Invoke();
        }
    }
}