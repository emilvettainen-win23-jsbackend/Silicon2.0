using Microsoft.JSInterop;

namespace Presentation.BlazorApp.SweetAlert;

public class AlertService
{
    private readonly IJSRuntime _jsRuntime;

    public AlertService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task ShowAlert(string icon, string title)
    {
        await _jsRuntime.InvokeVoidAsync("showAlertMessages", icon, title);
    }
}
