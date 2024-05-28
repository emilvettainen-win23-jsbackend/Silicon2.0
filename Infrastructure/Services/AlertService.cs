using Infrastructure.Models;

namespace Infrastructure.Services;

public class AlertService
{
    public event Action<AlertModel>? OnShow;
    public event Action? OnHide;

    public void ShowAlert(string message, string icon)
    {
        var alert = new AlertModel { Message = message, Icon = icon };
        OnShow?.Invoke(alert);
        _ = HideAlertAfterDelay();
    }

    private async Task HideAlertAfterDelay()
    {
        await Task.Delay(3000);
        OnHide?.Invoke();
    }
}
