namespace Infrastructure.Helpers.SweetAlert;

public class MessageService
{
    public string? Message { get; private set; }
    public string? Icon { get; private set; }

    public void SetAlert(string icon, string message)
    {
        Icon = icon;
        Message = message;
    }

    public void ClearAlert()
    {
        Icon = null;
        Message = null;
    }
}
