

namespace Infrastructure.Services;

public class DarkModeService
{
    public event Func<Task>? OnChangeAsync;

    public bool IsDarkMode { get; private set; }

    public async Task SetDarkMode(bool isDarkMode)
    {
        Console.WriteLine($"Attempting to set IsDarkMode from {IsDarkMode} to {isDarkMode}");
        if (IsDarkMode != isDarkMode)
        {
            IsDarkMode = isDarkMode;
            Console.WriteLine("IsDarkMode value changed. Notifying change.");
            await NotifyStateChangedAsync();
        }
        else
        {
            Console.WriteLine("No change in IsDarkMode value. No notification sent.");
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