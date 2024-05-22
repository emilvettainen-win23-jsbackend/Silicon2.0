namespace Presentation.BlazorApp.Helper
{
    public class MessageService
    {
        public MessageModel? Message { get; set; }
    }

    public class MessageModel
    {
        public string? Message { get; set; }
        public string? Icon { get; set; }
    }
}
